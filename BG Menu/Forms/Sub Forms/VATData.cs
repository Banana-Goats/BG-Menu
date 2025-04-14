using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Xml.Linq;
using Newtonsoft.Json;
using Sap.Data.Hana;
using Task = System.Threading.Tasks.Task;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Document = Spire.Doc.Document;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class VATData : Form
    {
        private string hanaConnectionString;

        private SemaphoreSlim _mergeSemaphore = new SemaphoreSlim(1);

        public VATData()
        {
            InitializeComponent();
            hanaConnectionString = ConfigurationManager.ConnectionStrings["Hana"].ConnectionString;
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            ProcessVatForms();
        }

        private async void ProcessVatForms()
        {
            // Set up UI elements
            progressBarSAP.Style = ProgressBarStyle.Marquee;
            progressBarSAP.Visible = true;
            button1.Enabled = false;

            // *********************
            // Use Previous Day for Dates
            // *********************
            DateTime previousDay = DateTime.Now.Date.AddDays(-1);
            string startDate = previousDay.ToString("d-MMM-yyyy");
            string endDate = previousDay.ToString("d-MMM-yyyy");

            // Build the API URL with the previous-day values.
            string requestUrl = $"https://ableworldapp.apollyon.online/API_Webservice/AbleWebServiceShared.asmx/ReturnCompletedOrderVATExemptCustomers?JSONString={{\"Authentication_Key\": \"2MzbGDerb5Qr7GSNpHEDPLfKu5pbuvjCkSGirqd5\", \"start_date\": \"{startDate}\", \"end_date\": \"{endDate}\"}}";
            textBox2.Text = requestUrl;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var handler = new HttpClientHandler())
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.32.0");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Remove any invalid characters.
                    foreach (var invalid in new string[] { "&" })
                    {
                        responseBody = responseBody.Replace(invalid, "");
                    }

                    var xdoc = XDocument.Parse(responseBody);
                    string jsonString = xdoc.Root?.Value;
                    jsonString = jsonString.Replace(@"\", @"\\");
                    var rootObject = JsonConvert.DeserializeObject<Root>(jsonString);

                    if (rootObject?.vat_exempt_collection != null)
                    {
                        foreach (var record in rootObject.vat_exempt_collection)
                        {
                            record.customer_surname = record.customer_surname?.Trim();
                        }
                    }

                    if (rootObject?.vat_exempt_collection != null && rootObject.vat_exempt_collection.Count > 0)
                    {
                        var bindingList = new BindingList<VatExempt>(rootObject.vat_exempt_collection);
                        dataGridView1.AutoGenerateColumns = true;
                        dataGridView1.DataSource = bindingList;

                        string[] columnsToRemove =
                        {
                            "created_time",
                            "customer_email",
                            "bespoke_form_reponse_3",
                            "bespoke_form_reponse_4",
                            "bespoke_form_reponse_5",
                            "bespoke_form_reponse_6"
                        };
                        foreach (string colName in columnsToRemove)
                        {
                            if (dataGridView1.Columns.Contains(colName))
                                dataGridView1.Columns.Remove(colName);
                        }

                        // Rename columns as needed.
                        var renameMap = new Dictionary<string, string>()
                        {
                            { "customer_title",         "Title" },
                            { "customer_forenames",     "FirstName" },
                            { "customer_surname",       "Surname" },
                            { "customer_address1",      "Address 1" },
                            { "customer_address2",      "Address 2" },
                            // City is not needed in the mail merge.
                            { "customer_county",        "County" },
                            { "customer_postcode",      "Postcode" },
                            { "bespoke_form_reponse_1", "Condition" },
                            { "bespoke_form_reponse_2", "ConditionDetail" },
                            { "bespoke_form_submitted", "FromDate" }
                        };
                        foreach (var kvp in renameMap)
                        {
                            string originalName = kvp.Key;
                            string newName = kvp.Value;
                            if (dataGridView1.Columns.Contains(originalName))
                            {
                                dataGridView1.Columns[originalName].Name = newName;
                                dataGridView1.Columns[newName].HeaderText = newName;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No items in vat_exempt_collection");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving API data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarSAP.Style = ProgressBarStyle.Blocks;
                progressBarSAP.Value = progressBarSAP.Maximum;
                // Do not re-enable the button here because we are chaining the processes.
            }

            // *********************
            // Sequentially execute the other processes:
            // *********************
            try
            {
                await UpdateCardCodesAsync();
                await MailMergeDocumentsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during processing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Now it is safe to re-enable button1.
                button1.Enabled = true;
            }
        }


        private async Task UpdateCardCodesAsync()
        {
            // Run the updates on a background task as before.
            await Task.Run(async () =>
            {
                var rows = dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();
                int totalPass1 = rows.Count, processedCount = 0;
                this.Invoke(new Action(() =>
                {
                    progressBarSAP.Style = ProgressBarStyle.Blocks;
                    progressBarSAP.Minimum = 0;
                    progressBarSAP.Maximum = totalPass1;
                    progressBarSAP.Value = 0;
                }));
                List<Task> pass1Tasks = new List<Task>();
                foreach (var row in rows)
                {
                    string postcode = "";
                    this.Invoke(new Action(() =>
                    {
                        postcode = row.Cells["Postcode"].Value?.ToString();
                        labelCurrentPostcode.Text = "Processing: " + postcode;
                    }));

                    string surname = row.Cells["Surname"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(postcode))
                    {
                        this.Invoke(new Action(() => { row.Cells["CardCode"].Value = "Missing Surname/Postcode"; }));
                        processedCount++;
                        this.Invoke(new Action(() => { progressBarSAP.Value = processedCount; }));
                        continue;
                    }
                    var task = UpdateRowCardCodeAsync(row, surname, postcode).ContinueWith(t =>
                    {
                        processedCount++;
                        this.Invoke(new Action(() => { progressBarSAP.Value = processedCount; }));
                    });
                    pass1Tasks.Add(task);
                }
                await Task.WhenAll(pass1Tasks);

                // Pass 2: Try a modified postcode format
                var rowsForPass2 = rows.Where(r => string.IsNullOrWhiteSpace(r.Cells["CardCode"].Value?.ToString())).ToList();
                int totalPass2 = rowsForPass2.Count;
                processedCount = 0;
                this.Invoke(new Action(() =>
                {
                    progressBarSAP.Minimum = 0;
                    progressBarSAP.Maximum = totalPass2;
                    progressBarSAP.Value = 0;
                }));
                List<Task> pass2Tasks = new List<Task>();
                foreach (var row in rowsForPass2)
                {
                    string surname = row.Cells["Surname"].Value?.ToString();
                    string postcode = row.Cells["Postcode"].Value?.ToString();
                    this.Invoke(new Action(() => { labelCurrentPostcode.Text = "Retrying: " + postcode; }));
                    if (!string.IsNullOrWhiteSpace(postcode) && !postcode.Contains(" ") && postcode.Length > 3)
                    {
                        string modifiedPostcode = postcode.Substring(0, postcode.Length - 3) + " " + postcode.Substring(postcode.Length - 3);
                        var task = UpdateRowCardCodeAsync(row, surname, modifiedPostcode, true, postcode).ContinueWith(t =>
                        {
                            processedCount++;
                            this.Invoke(new Action(() => { progressBarSAP.Value = processedCount; }));
                        });
                        pass2Tasks.Add(task);
                    }
                    else
                    {
                        processedCount++;
                        this.Invoke(new Action(() => { progressBarSAP.Value = processedCount; }));
                    }
                }
                await Task.WhenAll(pass2Tasks);

                // Pass 3: Phone check on rows with multiple card codes
                var rowsForPass3 = rows.Where(r =>
                {
                    string cc = r.Cells["CardCode"].Value?.ToString();
                    return !string.IsNullOrWhiteSpace(cc) && cc.Contains(",");
                }).ToList();
                int totalPass3 = rowsForPass3.Count;
                processedCount = 0;
                this.Invoke(new Action(() =>
                {
                    progressBarSAP.Minimum = 0;
                    progressBarSAP.Maximum = totalPass3;
                    progressBarSAP.Value = 0;
                }));
                List<Task> pass3Tasks = new List<Task>();
                foreach (var row in rowsForPass3)
                {
                    string postcode = row.Cells["Postcode"].Value?.ToString();
                    this.Invoke(new Action(() => { labelCurrentPostcode.Text = "Phone check for: " + postcode; }));
                    var task = UpdateRowCardCodeWithPhoneCheckAsync(row).ContinueWith(t =>
                    {
                        processedCount++;
                        this.Invoke(new Action(() => { progressBarSAP.Value = processedCount; }));
                    });
                    pass3Tasks.Add(task);
                }
                await Task.WhenAll(pass3Tasks);
            });
            // Remove rows that did not get a CardCode
            this.Invoke(new Action(() => RemoveRowsWithoutCardCode()));
        }

        private async Task MailMergeDocumentsAsync()
        {
            // Hardcoded template and output folder paths.
            string templatePath = @"\\marketingnas\Phone System\JOB_VAT_Template_Internet.docx";
            string outputDir = @"\\marketingnas\Phone System\JOB_VAT_Forms_Internet_Auto";

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            List<Task> tasks = new List<Task>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;
                VatExempt data = row.DataBoundItem as VatExempt;
                if (data == null)
                    continue;

                // Prepare the data for replacement.
                string accountName = $"{data.customer_forenames?.Trim()} {data.customer_surname?.Trim()}";
                string cardCode = data.CardCode;
                string street = data.customer_address1;
                string town = data.customer_address2;
                string county = data.customer_county;
                string postcode = data.customer_postcode;
                string phone = data.customer_phone;
                string email = data.customer_email;
                string condition = data.bespoke_form_reponse_1;
                if (!string.IsNullOrWhiteSpace(condition) &&
                    condition.Trim().Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    condition = data.bespoke_form_reponse_2;
                }
                string formDate = data.bespoke_form_submitted;

                // Build the dictionary of replacements.
                Dictionary<string, string> replacements = new Dictionary<string, string>()
                {
                    { "{Account Name}", accountName },
                    { "{Card Code}", cardCode },
                    { "{Street/Block}", street },
                    { "{Town}", town },
                    { "{County}", county },
                    { "{Post}", postcode },
                    { "{Phone Number}", phone },
                    { "{Email}", email },
                    { "{Other Condition}", condition },
                    { "{Form Date}", formDate }
                };

                tasks.Add(CreateMergedDocumentTask_OpenXml(templatePath, outputDir, cardCode, replacements));
            }

            await Task.WhenAll(tasks);
        }

        // Mail Merge

        private Task CreateMergedDocumentTask_OpenXml(string templatePath, string outputDir, string cardCode, Dictionary<string, string> replacements)
        {
            var tcs = new TaskCompletionSource<object>();
            _mergeSemaphore.Wait();
            Thread thread = new Thread(() =>
            {
                try
                {
                    CreateMergedDocument_OpenXml(templatePath, outputDir, cardCode, replacements);
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                finally
                {
                    _mergeSemaphore.Release();
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        private void CreateMergedDocument_OpenXml(string templatePath, string outputDir, string cardCode, Dictionary<string, string> replacements)
        {
            // Make a temporary copy of the template
            string tempTemplate = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
            File.Copy(templatePath, tempTemplate, true);

            // Perform placeholder replacement
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(tempTemplate, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var kvp in replacements)
                    {
                        if (text.Text.Contains(kvp.Key))
                        {
                            text.Text = text.Text.Replace(kvp.Key, kvp.Value);
                        }
                    }
                }
                wordDoc.MainDocumentPart.Document.Save();
            }

            // Sanitize the cardCode to remove any invalid file name chars
            string sanitizedCardCode = new string(
                cardCode.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());

            // Build the final output paths
            string outputDocxPath = Path.Combine(outputDir, $"JOB_VAT_{sanitizedCardCode}.docx");
            string outputPdfPath = Path.Combine(outputDir, $"JOB_VAT_{sanitizedCardCode}.pdf");

            // Overwrite any existing file with the same name
            if (File.Exists(outputDocxPath))
            {
                File.Delete(outputDocxPath);
            }
            if (File.Exists(outputPdfPath))
            {
                File.Delete(outputPdfPath);
            }

            // Move the temporary file to the output location (this may throw IOException)
            try
            {
                File.Move(tempTemplate, outputDocxPath);
            }
            catch (IOException ioEx)
            {                
                return; // Skip conversion & cleanup for this one.
            }

            // Convert DOCX to PDF and cleanup
            try
            {
                ConvertDocxToPdf(outputDocxPath, outputPdfPath);
                // Optionally delete the DOCX file after conversion if not needed.
                if (File.Exists(outputDocxPath))
                {
                    File.Delete(outputDocxPath);
                }
            }
            catch (Exception ex)
            {
                return; 
            }
        }

        private void ConvertDocxToPdf(string docxPath, string pdfPath)
        {
            // Initialize a new Spire.Doc.Document instance.
            Document document = new Document();
            document.LoadFromFile(docxPath);
            // Save the document as a PDF.
            document.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);
        }

        //Card Code Data

        private async Task UpdateRowCardCodeAsync(DataGridViewRow row, string surname, string postcode, bool isRetry = false, string originalPostcode = "")
        {
            string cardCodes = await GetCardCodeForRowAsync(surname, postcode);
            this.Invoke(new Action(() =>
            {
                var item = row.DataBoundItem as VatExempt;
                if (item != null)
                {
                    item.CardCode = cardCodes;
                }
            }));
        }

        private async Task<string> GetCardCodeForRowAsync(string surname, string postcode)
        {
            List<string> cardCodes = new List<string>();
            try
            {
                using (var connection = new HanaConnection(hanaConnectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT ""CardCode""
                        FROM ""SBO_AWUK_NEWLIVE"".""OCRD""
                        WHERE UPPER(""CardName"") LIKE UPPER(:surnamePattern)
                          AND ""CardFName"" = :postcode";
                    using (var command = new HanaCommand(query, connection))
                    {
                        command.Parameters.Add(new HanaParameter("surnamePattern", $"%{surname}%"));
                        command.Parameters.Add(new HanaParameter("postcode", postcode));
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (!reader.IsDBNull(0))
                                    cardCodes.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "Error";
            }
            return string.Join(", ", cardCodes);
        }

        private async Task UpdateRowCardCodeWithPhoneCheckAsync(DataGridViewRow row)
        {
            string cardCodes = "";
            string customerPhone = "";
            this.Invoke(new Action(() =>
            {
                cardCodes = row.Cells["CardCode"].Value?.ToString();
                customerPhone = row.Cells["customer_phone"].Value?.ToString();
            }));

            if (string.IsNullOrWhiteSpace(customerPhone))
                return;

            string normalizedCustomerPhone = new string(customerPhone.Where(char.IsDigit).ToArray());
            string[] codes = cardCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> matchingCodes = new List<string>();

            foreach (var code in codes)
            {
                string trimmedCode = code.Trim();
                string phone1 = await GetPhone1AsyncParameterized(trimmedCode);
                if (!string.IsNullOrWhiteSpace(phone1))
                {
                    string normalizedSAPPhone = new string(phone1.Where(char.IsDigit).ToArray());
                    if (normalizedSAPPhone == normalizedCustomerPhone)
                        matchingCodes.Add(trimmedCode);
                }
            }

            if (matchingCodes.Count > 0)
            {
                string matchedCode = matchingCodes[0];
                this.Invoke(new Action(() =>
                {
                    var item = row.DataBoundItem as VatExempt;
                    if (item != null)
                        item.CardCode = matchedCode;
                }));
            }
        }

        private async Task<string> GetPhone1AsyncParameterized(string cardCode)
        {
            try
            {
                using (var connection = new HanaConnection(hanaConnectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT ""Phone1""
                        FROM ""SBO_AWUK_NEWLIVE"".""OCRD""
                        WHERE ""CardCode"" = :cardCode";
                    using (var command = new HanaCommand(query, connection))
                    {
                        command.Parameters.Add(new HanaParameter("cardCode", cardCode));
                        object result = await command.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                            return result.ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        private void RemoveRowsWithoutCardCode()
        {
            var bindingList = dataGridView1.DataSource as BindingList<VatExempt>;
            if (bindingList != null)
            {
                for (int i = bindingList.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(bindingList[i].CardCode))
                    {
                        bindingList.RemoveAt(i);
                    }
                }
            }
        }

        // ====================================
        // Classes for deserializing API data
        // ====================================
        public class Root
        {
            public string message { get; set; }
            public List<VatExempt> vat_exempt_collection { get; set; }
        }

        public class VatExempt
        {
            public string created_time { get; set; }
            public string customer_title { get; set; }
            public string customer_forenames { get; set; }
            public string customer_surname { get; set; }
            public string customer_address1 { get; set; }
            public string customer_address2 { get; set; }
            public string customer_city { get; set; }
            public string customer_county { get; set; }
            public string customer_postcode { get; set; }
            public string customer_phone { get; set; }
            public string customer_email { get; set; }
            public string bespoke_form_reponse_1 { get; set; }
            public string bespoke_form_reponse_2 { get; set; }
            public string bespoke_form_reponse_3 { get; set; }
            public string bespoke_form_reponse_4 { get; set; }
            public string bespoke_form_reponse_5 { get; set; }
            public string bespoke_form_submitted { get; set; }
            public string CardCode { get; set; }
        }
    }
}
