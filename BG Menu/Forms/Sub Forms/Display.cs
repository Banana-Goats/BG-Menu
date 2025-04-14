using DocumentFormat.OpenXml.Packaging;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Sap.Data.Hana;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net;
using System.Xml.Linq;
using Timer = System.Windows.Forms.Timer;
using Task = System.Threading.Tasks.Task;
using DocumentFormat.OpenXml.Wordprocessing;
using Document = Spire.Doc.Document;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Control = System.Windows.Forms.Control;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class Display : Form
    {
        private Timer checkTimer = new Timer();
        private bool isProcessing = false;
        private DateTime lastProcessedDate = DateTime.MinValue;

        private string hanaConnectionString;

        private SemaphoreSlim _mergeSemaphore = new SemaphoreSlim(1);

        private Timer refreshTimer;
        private List<TileData> _currentData = new List<TileData>();

        private Timer dailyFolderProcessingTimer;

        private DateTime? _lastRecordingProcessedDate;
        private int _totalCallsProcessed = 0;

        public Display()
        {
            InitializeComponent();

            hanaConnectionString = ConfigurationManager.ConnectionStrings["Hana"].ConnectionString;

            flowLayoutPanelTiles.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelTiles.WrapContents = false;
            flowLayoutPanelTiles.Resize += FlowLayoutPanelTiles_Resize;

            refreshTimer = new Timer { Interval = 10000 };
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();

            SetupDailyFolderProcessingTimer();

            LoadTiles();
            UpdateRecordingsSummaryTile();

            //ProcessRecordingFoldersAsync();
            //ProcessVatForms();

        }

        private void SetupDailyFolderProcessingTimer()
        {
            // Set the timer to tick every 10 seconds (10,000 milliseconds)
            checkTimer.Interval = 10000;
            checkTimer.Tick += CheckTimer_Tick;
            checkTimer.Start();
        }

        private async void CheckTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            if ((now.Hour == 9) && (lastProcessedDate.Date != now.Date) && !isProcessing)
            {
                isProcessing = true;

                AppendProgress($"Folder processing starting at {now:HH:mm:ss}.");

                await ProcessRecordingFoldersAsync();
                await ProcessVatForms();

                lastProcessedDate = now.Date;
                isProcessing = false; 

                AppendProgress($"Folder processing completed at {DateTime.Now:HH:mm:ss}.");
            }
        }

        #region Network Manager

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadTiles();
        }

        private void LoadTiles()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            string query = @"
                SELECT Name, Store, ISP, Mobile 
                FROM dbo.TBPC 
                WHERE Store LIKE '%Till%'
                  AND (DATEDIFF(MINUTE, Pulse_Time, GETDATE()) > 5 OR ISP = 'ISP Failed' OR Mobile = 1)";

            List<TileData> newData = new List<TileData>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        newData.Add(new TileData
                        {
                            Name = reader["Name"].ToString(),
                            Store = reader["Store"].ToString(),
                            ISP = reader["ISP"].ToString(),
                            Mobile = reader["Mobile"] != DBNull.Value ? Convert.ToBoolean(reader["Mobile"]) : false
                        });
                    }
                }
            }

            if (newData.SequenceEqual(_currentData))
            {
                return;
            }

            _currentData = newData;

            flowLayoutPanelTiles.SuspendLayout();
            flowLayoutPanelTiles.Controls.Clear();
            foreach (var data in _currentData)
            {
                Panel tile = CreateTile(data.Name, data.Store, data.ISP, data.Mobile);
                tile.Width = flowLayoutPanelTiles.ClientSize.Width - tile.Margin.Horizontal;
                flowLayoutPanelTiles.Controls.Add(tile);
            }
            flowLayoutPanelTiles.ResumeLayout();
        }

        private Panel CreateTile(string name, string store, string isp, bool mobile)
        {
            Color tileColor = mobile ? Color.LightGreen : (isp == "ISP Failed" ? Color.Yellow : Color.Tomato);

            Panel tile = new Panel
            {
                Height = 70,
                BackColor = tileColor,
                Margin = new Padding(5)
            };

            Label lblName = new Label
            {
                Text = "" + name,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };

            Label lblStore = new Label
            {
                Text = "" + store,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 30)
            };

            tile.Controls.Add(lblName);
            tile.Controls.Add(lblStore);

            int rightMargin = 10;

            if (mobile)
            {
                Label lbl4G = new Label
                {
                    Text = "4G",
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    AutoSize = true,
                    Padding = new Padding(3)
                };

                lbl4G.Paint += (s, e) =>
                {
                    using (Pen borderPen = new Pen(Color.Black))
                    {
                        e.Graphics.DrawRectangle(borderPen, 0, 0, lbl4G.Width - 1, lbl4G.Height - 1);
                    }
                };

                tile.Controls.Add(lbl4G);

                lbl4G.Location = new Point(
                    tile.ClientSize.Width - rightMargin - lbl4G.Width,
                    (tile.ClientSize.Height - lbl4G.Height) / 2
                );

                lbl4G.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                rightMargin += lbl4G.Width + 10;
            }

            if (isp == "ISP Failed")
            {
                Label lblDNS = new Label
                {
                    Text = "DNS",
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    AutoSize = true,
                    Padding = new Padding(3)
                };

                lblDNS.Paint += (s, e) =>
                {
                    using (Pen borderPen = new Pen(Color.Black))
                    {
                        e.Graphics.DrawRectangle(borderPen, 0, 0, lblDNS.Width - 1, lblDNS.Height - 1);
                    }
                };

                tile.Controls.Add(lblDNS);

                lblDNS.Location = new Point(
                    tile.ClientSize.Width - rightMargin - lblDNS.Width,
                    (tile.ClientSize.Height - lblDNS.Height) / 2
                );

                lblDNS.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                rightMargin += lblDNS.Width + 10;
            }

            return tile;
        }


        private void FlowLayoutPanelTiles_Resize(object sender, EventArgs e)
        {
            foreach (Control tile in flowLayoutPanelTiles.Controls)
            {
                tile.Width = flowLayoutPanelTiles.ClientSize.Width - tile.Margin.Horizontal;
            }
        }

        public class TileData
        {
            public string Name { get; set; }
            public string Store { get; set; }
            public string ISP { get; set; }
            public bool Mobile { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is TileData other)
                {
                    return Name == other.Name &&
                           Store == other.Store &&
                           ISP == other.ISP &&
                           Mobile == other.Mobile;
                }
                return false;
            }

            public override int GetHashCode() => (Name, Store, ISP, Mobile).GetHashCode();
        }

        #endregion

        #region Daily Folder Processing        

        private async Task ProcessRecordingFoldersAsync()
        {
            try
            {
                string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";

                if (!Directory.Exists(targetDirectory))
                {
                    AppendProgress($"Target directory not found: {targetDirectory}", false, Color.Red);
                    return;
                }

                // Find folders starting with "RecordingFiles-".
                var directories = await Task.Run(() =>
                    Directory.GetDirectories(targetDirectory)
                             .Where(dir => Path.GetFileName(dir).StartsWith("RecordingFiles-"))
                             .ToList()
                );

                foreach (var dir in directories)
                {
                    string folderName = Path.GetFileName(dir);
                    // Generate new folder name based on your existing logic.
                    string newName = GetNewFolderName(folderName);
                    if (string.IsNullOrEmpty(newName))
                    {
                        AppendProgress($"Folder format not recognized: {folderName}", false, Color.Orange);
                        continue;
                    }

                    string newFullPath = Path.Combine(targetDirectory, newName);
                    try
                    {
                        // Rename the folder.
                        Directory.Move(dir, newFullPath);
                        AppendProgress($"Folder renamed to: {newName}");

                        // Process all WAV files sequentially in the renamed folder.
                        int processedCount = await ProcessWavFilesSequentiallyAsync(newFullPath);

                        _lastRecordingProcessedDate = DateTime.Now;
                        _totalCallsProcessed = processedCount;
                        UpdateRecordingsSummaryTile();

                        // Move the processed folder into its monthly folder.
                        MoveProcessedFolder(newFullPath);
                    }
                    catch (Exception ex)
                    {
                        AppendProgress($"Error processing folder {folderName}: {ex.Message}", false, Color.Red);
                    }
                }

                // Ensure any remaining folders in DD-MM-YYYY format are moved.
                MoveRemainingProcessedFolders();
            }
            catch (Exception ex)
            {
                AppendProgress($"Error in ProcessRecordingFoldersAsync: {ex.Message}", false, Color.Red);
            }
        }

        // Renames folder based on expected format.
        private string GetNewFolderName(string folderName)
        {
            var parts = folderName.Split('-');
            if (parts.Length < 3)
                return null;

            string datePart = parts.Last();
            if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
            {
                string formattedDate = date.ToString("dd-MM-yyyy");

                if (folderName.Contains("Daily Backup"))
                {
                    return $"{formattedDate}";
                }
                else
                {
                    string remainingPart = string.Join(" - ", parts.Skip(1).Take(parts.Length - 2));
                    return $"{remainingPart} - {formattedDate}";
                }
            }

            return null;
        }

        // Processes WAV files inside a folder and returns the number of files processed.
        private async Task<int> ProcessWavFilesSequentiallyAsync(string folderPath)
        {
            var wavFiles = Directory.GetFiles(folderPath, "*.wav");
            int totalFiles = wavFiles.Length;
            int counter = 0;

            // Clear any previous log entries.
            logEntries.Clear();
            currentLogIndex = -1;
            RefreshLogDisplay();

            foreach (var wavFile in wavFiles)
            {
                counter++;

                var (alreadyProcessed, newFileName) = await ProcessWavFileAsync(wavFile);
                string message = $"{counter} / {totalFiles} - {newFileName} " +
                                 (alreadyProcessed ? "File already processed" : "Processed");

                if (alreadyProcessed)
                {
                    logEntries.Add(new LogEntry { Message = message, Color = Color.Red });
                    currentLogIndex = -1;
                }
                else
                {
                    if (currentLogIndex == -1)
                    {
                        logEntries.Add(new LogEntry { Message = message, Color = Color.YellowGreen });
                        currentLogIndex = logEntries.Count - 1;
                    }
                    else
                    {
                        logEntries[currentLogIndex].Message = message;
                    }
                }
                RefreshLogDisplay();
            }

            logEntries.Add(new LogEntry { Message = "All files processed", Color = Color.White });
            RefreshLogDisplay();

            return counter;
        }

        // Converts a WAV file to MP3 and cleans up.
        private async Task<(bool alreadyProcessed, string newFileName)> ProcessWavFileAsync(string wavFile)
        {
            string newFileName = GetNewMp3FileName(wavFile);
            string processingFile = wavFile + ".processing";
            try
            {
                // Attempt to lock the file.
                File.Move(wavFile, processingFile);
            }
            catch (Exception)
            {
                // If we cannot lock it, assume it has already been processed.
                return (true, newFileName);
            }

            string mp3File = Path.ChangeExtension(processingFile, ".mp3");
            try
            {
                using (var reader = new NAudio.Wave.AudioFileReader(processingFile))
                using (var writer = new NAudio.Lame.LameMP3FileWriter(mp3File, reader.WaveFormat, NAudio.Lame.LAMEPreset.VBR_90))
                {
                    await Task.Run(() => reader.CopyTo(writer));
                }

                string finalMp3FilePath = Path.Combine(Path.GetDirectoryName(mp3File), newFileName);
                File.Move(mp3File, finalMp3FilePath);
            }
            catch (Exception)
            {
                // Handle conversion error as needed.
            }
            finally
            {
                if (File.Exists(processingFile))
                {
                    try { File.Delete(processingFile); } catch { }
                }
            }

            return (false, newFileName);
        }

        // Generates a new MP3 file name based on the WAV file name.
        private string GetNewMp3FileName(string wavFilePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(wavFilePath);
            string[] parts = fileName.Split('-');
            if (parts.Length >= 5 && DateTime.TryParseExact(parts[0], "yyyyMMddHHmmss", null, DateTimeStyles.None, out DateTime timestamp))
            {
                string formattedTimestamp = timestamp.ToString("dd-MM-yyyy HH-mm");
                string caller = parts[2];
                string callee = parts[3];
                return $"{formattedTimestamp} ( {caller} - {callee} ).mp3";
            }
            else
            {
                return fileName + ".mp3";
            }
        }

        private void MoveProcessedFolder(string processedFolderPath)
        {
            string folderName = Path.GetFileName(processedFolderPath);
            if (DateTime.TryParseExact(folderName, "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime folderDate))
            {
                string monthFolderName = folderDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
                string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";
                string destinationFolderPath = Path.Combine(targetDirectory, monthFolderName);

                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }

                string finalDestination = Path.Combine(destinationFolderPath, folderName);
                if (Directory.Exists(finalDestination))
                {
                    AppendProgress($"Folder {folderName} already exists in {monthFolderName}.", false, Color.Orange);
                }
                else
                {
                    Directory.Move(processedFolderPath, finalDestination);
                    AppendProgress($"Moved folder {folderName} to {monthFolderName}.", false, Color.Green);
                }
            }
            else
            {
                AppendProgress($"Folder {folderName} is not in the expected DD-MM-YYYY format; not moved.", false, Color.Orange);
            }
        }

        private void MoveRemainingProcessedFolders()
        {
            string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";
            var directories = Directory.GetDirectories(targetDirectory);
            foreach (var dir in directories)
            {
                string folderName = Path.GetFileName(dir);

                if (DateTime.TryParseExact(folderName, "dd-MM-yyyy", null, DateTimeStyles.None, out _))
                {
                    MoveProcessedFolder(dir);
                }
            }
        }

        private List<LogEntry> logEntries = new List<LogEntry>();
        private int currentLogIndex = -1;

        private void RefreshLogDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshLogDisplay));
                return;
            }

            progressTextBox.Clear();
            foreach (var entry in logEntries)
            {
                progressTextBox.SelectionStart = progressTextBox.TextLength;
                progressTextBox.SelectionColor = entry.Color;
                progressTextBox.AppendText(entry.Message + Environment.NewLine);
            }
            progressTextBox.SelectionColor = progressTextBox.ForeColor;
            progressTextBox.ScrollToCaret();
        }

        private void AppendProgress(string message, bool overwrite = false, Color? textColor = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendProgress(message, overwrite, textColor)));
                return;
            }

            RichTextBox richTextBox = progressTextBox as RichTextBox;
            if (richTextBox != null)
            {
                if (textColor.HasValue)
                {
                    richTextBox.SelectionStart = richTextBox.TextLength;
                    richTextBox.SelectionLength = 0;
                    richTextBox.SelectionColor = textColor.Value;
                }

                if (overwrite && richTextBox.Lines.Length > 0)
                {
                    var lines = richTextBox.Lines.ToList();
                    lines[lines.Count - 1] = message;
                    richTextBox.Lines = lines.ToArray();
                }
                else
                {
                    richTextBox.AppendText(message + Environment.NewLine);
                }

                richTextBox.SelectionColor = richTextBox.ForeColor;
                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
            }
            else
            {
                progressTextBox.AppendText(message + Environment.NewLine);
            }
        }

        private void UpdateRecordingsSummaryTile()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateRecordingsSummaryTile));
                return;
            }

            lblLastProcessed.Text = _lastRecordingProcessedDate.HasValue
                ? "Last Processed: " + _lastRecordingProcessedDate.Value.ToString("dd-MM-yyyy HH:mm:ss")
                : "Last Processed: Not yet processed";

            lblCallsProcessed.Text = "Calls Processed In Last Batch: " + _totalCallsProcessed;
        }

        class LogEntry
        {
            public string Message { get; set; }
            public Color Color { get; set; }
        }

        #endregion

        #region Vat Forms

        private async void button1_Click(object sender, EventArgs e)
        {
            // Create the date range form on the fly.
            using (Form dateForm = new Form())
            {
                dateForm.Text = "Select Date Range";
                dateForm.Size = new Size(300, 180);
                dateForm.StartPosition = FormStartPosition.CenterParent;
                dateForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dateForm.MaximizeBox = false;
                dateForm.MinimizeBox = false;

                // Create the start date label and picker.
                Label lblStart = new Label() { Text = "Start Date:", Location = new Point(10, 20), AutoSize = true };
                DateTimePicker dtpStart = new DateTimePicker() { Location = new Point(100, 15), Format = DateTimePickerFormat.Short };

                // Create the end date label and picker.
                Label lblEnd = new Label() { Text = "End Date:", Location = new Point(10, 60), AutoSize = true };
                DateTimePicker dtpEnd = new DateTimePicker() { Location = new Point(100, 55), Format = DateTimePickerFormat.Short };

                // Create OK and Cancel buttons.
                Button btnOK = new Button() { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(40, 100) };
                Button btnCancel = new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(150, 100) };

                // Add controls to the form.
                dateForm.Controls.Add(lblStart);
                dateForm.Controls.Add(dtpStart);
                dateForm.Controls.Add(lblEnd);
                dateForm.Controls.Add(dtpEnd);
                dateForm.Controls.Add(btnOK);
                dateForm.Controls.Add(btnCancel);

                dateForm.AcceptButton = btnOK;
                dateForm.CancelButton = btnCancel;

                // Show the dialog.
                DialogResult result = dateForm.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    // Format the selected dates.
                    string startDate = dtpStart.Value.ToString("d-MMM-yyyy");
                    string endDate = dtpEnd.Value.ToString("d-MMM-yyyy");
                    await ProcessVatForms(startDate, endDate);
                }
                else
                {
                    // Call ProcessVatForms without parameters; defaulting to previous day values.
                    await ProcessVatForms();
                }
            }
        }

        private async Task ProcessVatForms(string startDate = null, string endDate = null)
        {
            // Set up UI status.
            progressBarSAP.Style = ProgressBarStyle.Marquee;
            progressBarSAP.Visible = true;
            button1.Enabled = false;

            // Declare an effective date variable to show what date is used.
            DateTime effectiveDate;

            // Use previous day values when no dates are provided.
            if (string.IsNullOrWhiteSpace(startDate) || string.IsNullOrWhiteSpace(endDate))
            {
                effectiveDate = DateTime.Now.Date.AddDays(-1);
                startDate = effectiveDate.ToString("d-MMM-yyyy");
                endDate = effectiveDate.ToString("d-MMM-yyyy");
            }
            else
            {
                // If dates are provided, try to parse the start date to set effectiveDate.
                if (!DateTime.TryParseExact(startDate, "d-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out effectiveDate))
                {
                    // If parsing fails, fallback to previous day.
                    effectiveDate = DateTime.Now.Date.AddDays(-1);
                }
            }

            // Build the API URL using the effective or provided date values.
            string requestUrl = $"https://ableworldapp.apollyon.online/API_Webservice/AbleWebServiceShared.asmx/ReturnCompletedOrderVATExemptCustomers?JSONString={{\"Authentication_Key\": \"2MzbGDerb5Qr7GSNpHEDPLfKu5pbuvjCkSGirqd5\", \"start_date\": \"{startDate}\", \"end_date\": \"{endDate}\"}}";

            List<VatExempt> vatRecords = new List<VatExempt>();

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
                        vatRecords = rootObject.vat_exempt_collection;
                    }
                    else
                    {
                        MessageBox.Show("No items in vat_exempt_collection");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving API data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                progressBarSAP.Style = ProgressBarStyle.Blocks;
                progressBarSAP.Value = progressBarSAP.Maximum;
            }

            try
            {
                await UpdateCardCodesAsync(vatRecords);
                await MailMergeDocumentsAsync(vatRecords);

                int processedCount = vatRecords.Count(r => !string.IsNullOrWhiteSpace(r.CardCode));
                // Use the effectiveDate for the display label.
                this.Invoke(new Action(() =>
                {
                    labelVatFormCount.Text = $"{effectiveDate.ToString("d-MMM-yyyy")}: {processedCount} forms processed.";
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during processing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private async Task UpdateCardCodesAsync(List<VatExempt> records)
        {
            // Pass 1: Retrieve card codes using the original postcode.
            var pass1Tasks = new List<Task>();
            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.customer_surname) || string.IsNullOrWhiteSpace(record.customer_postcode))
                {
                    record.CardCode = "Missing Surname/Postcode";
                }
                else
                {
                    pass1Tasks.Add(Task.Run(async () =>
                    {
                        record.CardCode = await GetCardCodeForRowAsync(record.customer_surname, record.customer_postcode);
                    }));
                }
            }
            await Task.WhenAll(pass1Tasks);

            // Pass 2: For records with no CardCode, try a modified postcode format.
            var pass2Tasks = new List<Task>();
            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.CardCode))
                {
                    string postcode = record.customer_postcode;
                    if (!string.IsNullOrWhiteSpace(postcode) && !postcode.Contains(" ") && postcode.Length > 3)
                    {
                        string modifiedPostcode = postcode.Substring(0, postcode.Length - 3) + " " + postcode.Substring(postcode.Length - 3);
                        pass2Tasks.Add(Task.Run(async () =>
                        {
                            record.CardCode = await GetCardCodeForRowAsync(record.customer_surname, modifiedPostcode);
                        }));
                    }
                }
            }
            await Task.WhenAll(pass2Tasks);

            // Pass 3: For records with multiple CardCodes (comma-separated), perform a phone check.
            var pass3Tasks = new List<Task>();
            foreach (var record in records)
            {
                if (!string.IsNullOrWhiteSpace(record.CardCode) && record.CardCode.Contains(","))
                {
                    pass3Tasks.Add(UpdateRecordCardCodeWithPhoneCheckAsync(record));
                }
            }
            await Task.WhenAll(pass3Tasks);
        }

        private async Task MailMergeDocumentsAsync(List<VatExempt> records)
        {
            // Filter records with a valid CardCode.
            var validRecords = records.Where(r => !string.IsNullOrWhiteSpace(r.CardCode)).ToList();

            // Hardcoded template and output folder paths.
            string templatePath = @"\\marketingnas\Phone System\JOB_VAT_Template_Internet.docx";
            string outputDir = @"\\marketingnas\Phone System\JOB_VAT_Forms_Internet_Auto";

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Configure the progress bar on the UI thread.
            progressBarSAP.Invoke((Action)(() =>
            {
                progressBarSAP.Style = ProgressBarStyle.Blocks;
                progressBarSAP.Minimum = 0;
                progressBarSAP.Maximum = validRecords.Count;
                progressBarSAP.Value = 0;
            }));

            // Process each valid record sequentially.
            foreach (var data in validRecords)
            {
                // Prepare the replacement data.
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

                var replacements = new Dictionary<string, string>()
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

                // Process the document conversion on a background STA thread.
                await CreateMergedDocumentTask_OpenXml(templatePath, outputDir, cardCode, replacements);

                // After each document is processed, update the progress bar on the UI thread.
                progressBarSAP.Invoke(new Action(() =>
                {
                    progressBarSAP.Value = Math.Min(progressBarSAP.Value + 1, progressBarSAP.Maximum);
                }));
            }
        }

        private Task CreateMergedDocumentTask_OpenXml(string templatePath, string outputDir, string cardCode, Dictionary<string, string> replacements)
        {
            var tcs = new TaskCompletionSource<object>();
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
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        private void CreateMergedDocument_OpenXml(string templatePath, string outputDir, string cardCode, Dictionary<string, string> replacements)
        {
            // Create a temporary copy of the template.
            string tempTemplate = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
            File.Copy(templatePath, tempTemplate, true);

            // Replace the placeholders.
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

            // Sanitize cardCode for file naming.
            string sanitizedCardCode = new string(cardCode.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
            string outputDocxPath = Path.Combine(outputDir, $"JOB_VAT_{sanitizedCardCode}.docx");
            string outputPdfPath = Path.Combine(outputDir, $"JOB_VAT_{sanitizedCardCode}.pdf");

            // Overwrite any existing files.
            if (File.Exists(outputDocxPath)) File.Delete(outputDocxPath);
            if (File.Exists(outputPdfPath)) File.Delete(outputPdfPath);

            // Move the temporary file to the output location.
            try
            {
                File.Move(tempTemplate, outputDocxPath);
            }
            catch (IOException)
            {
                return;
            }

            try
            {
                ConvertDocxToPdf(outputDocxPath, outputPdfPath);
                if (File.Exists(outputDocxPath))
                {
                    File.Delete(outputDocxPath);
                }
            }
            catch (Exception)
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

        private async Task UpdateRecordCardCodeWithPhoneCheckAsync(VatExempt record)
        {
            string cardCodes = record.CardCode;
            string customerPhone = record.customer_phone;
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
                record.CardCode = matchingCodes[0];
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

        #endregion

    }
}
