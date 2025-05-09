using BG_Menu.Class.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization; // For CultureInfo
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using BG_Menu.Data;
using System.Data;
using BG_Menu.Class.Sales_Summary;
using System.Net.NetworkInformation;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class NewCreditCard : Form
    {
        private SalesRepository salesRepository;
        private ContextMenuStrip loadButtonContextMenu;

        public NewCreditCard()
        {
            InitializeComponent();
            InitializeDataGridView();

            salesRepository = GlobalInstances.SalesRepository;

            SupplierConfigLoader.LoadSuppliers();

            InitializeContextMenu();

            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;

            PopulateReportPeriodComboBox();

            var rp = ComboMonth.SelectedItem?.ToString();
            LoadDataFromDatabase(rp);
        }

        private void InitializeContextMenu()
        {
            loadButtonContextMenu = new ContextMenuStrip();
            var reloadSuppliersItem = new ToolStripMenuItem("Reload Suppliers");
            reloadSuppliersItem.Click += ReloadSuppliersItem_Click;
            loadButtonContextMenu.Items.Add(reloadSuppliersItem);
            btnLoad.ContextMenuStrip = loadButtonContextMenu;
        }

        private void ReloadSuppliersItem_Click(object sender, EventArgs e)
        {
            try
            {
                SupplierConfigLoader.LoadSuppliers();
                RefreshSupplierDependentLogic();
                LogMessage("Suppliers successfully reloaded.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reloading suppliers: {ex.Message}", "Reload Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error reloading suppliers: {ex.Message}");
            }
        }

        private void RefreshSupplierDependentLogic()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                ApplyLogicForRow(row.Index);
            }
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();

            // ID (hidden)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "ID", HeaderText = "ID", Visible = false });
            // Description
            dataGridView1.Columns.Add("Description", "Description");
            // GL Account
            dataGridView1.Columns.Add("GLAccount", "GL Account");
            // Dimension
            dataGridView1.Columns.Add("Dimension", "Dimension");
            // GL Name
            dataGridView1.Columns.Add("GLName", "GL Name");
            // VAT (combo)
            dataGridView1.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "VAT",
                HeaderText = "VAT",
                Items = { "I1", "I2" },
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
            });
            // Total
            dataGridView1.Columns.Add("Total", "Total");
            // Value (read-only)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Value", HeaderText = "Value", ReadOnly = true });
            // IsModified (hidden)
            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { Name = "IsModified", HeaderText = "IsModified", Visible = false });
            // Invoice status
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Invoice", HeaderText = "Invoice", ReadOnly = true });
        }

        private void PopulateReportPeriodComboBox()
        {
            ComboMonth.Items.Clear();
            var reportPeriods = GetUniqueReportPeriodsFromDatabase();
            var currentWorkingMonth = CalculateCurrentWorkingMonth();

            if (!reportPeriods.Contains(currentWorkingMonth))
                reportPeriods.Add(currentWorkingMonth);

            reportPeriods = reportPeriods
                .Select(rp => new { Period = rp, Date = DateTime.ParseExact(rp, "MMMM yyyy", CultureInfo.InvariantCulture) })
                .OrderByDescending(x => x.Date)
                .Select(x => x.Period)
                .ToList();

            ComboMonth.Items.AddRange(reportPeriods.ToArray());
            ComboMonth.SelectedItem = currentWorkingMonth;
        }

        private List<string> GetUniqueReportPeriodsFromDatabase()
        {
            var periods = new List<string>();
            const string query = "SELECT DISTINCT ReportPeriod FROM CreditCardTransactions";

            try
            {
                var dt = salesRepository.ExecuteSqlQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    var rp = row["ReportPeriod"]?.ToString();
                    if (!string.IsNullOrEmpty(rp))
                        periods.Add(rp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving Report Periods:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error retrieving Report Periods: {ex.Message}");
            }

            return periods;
        }

        private string CalculateCurrentWorkingMonth()
        {
            var today = DateTime.Today;
            var workingMonthDate = today.Day >= 10 ? today.AddMonths(1) : today;
            return workingMonthDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                Title = "Select Invoice PDF Files",
                Multiselect = true
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            var reportPeriod = ComboMonth.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(reportPeriod))
            {
                MessageBox.Show("Please select a Report Period before importing.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var file in ofd.FileNames)
                ExtractDataFromPdfAndPopulate(file, reportPeriod);

            SaveDataToSqlServer(reportPeriod);
            PopulateReportPeriodComboBox();
            LoadDataFromDatabase(reportPeriod);
        }

        private void ExtractDataFromPdfAndPopulate(string filePath, string reportPeriod)
        {
            try
            {
                var text = ExtractTextFromPdf(filePath);
                var supplierName = DetectSupplier(text);

                if (supplierName == "Unknown Supplier")
                {
                    // pop up resolver
                    using var dlg = new SupplierResolverForm(
                    text,
                    SupplierConfigLoader.Suppliers);

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // 1) ensure the supplier exists or insert it
                        SupplierConfigLoader.AddOrUpdateSupplier(
                            dlg.SelectedSupplierName,
                            dlg.NewDetectionKeyword,
                            dlg.NewTotalRegex,
                            dlg.SelectedGLAccount,
                            dlg.SelectedGLName,
                            dlg.SelectedVATCode);

                        // 2) reload into memory
                        SupplierConfigLoader.LoadSuppliers();

                        // 3) retry detection
                        supplierName = dlg.SelectedSupplierName;
                    }
                    else
                    {
                        // user cancelled → skip
                        LogMessage($"Import cancelled for {filePath} (no supplier).");
                        return;
                    }
                }

                var supplier = SupplierConfigLoader.Suppliers.FirstOrDefault(s => s.Name.Equals(supplierName, StringComparison.OrdinalIgnoreCase));
                if (supplier == null)
                {
                    MessageBox.Show($"Supplier config for '{supplierName}' not found. Skipping.", "Config Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMessage($"Missing config for '{supplierName}'.");
                    return;
                }

                var totalStr = ExtractTotalFromPdf(text, supplier)?.Replace(",", "");
                if (!decimal.TryParse(totalStr, out var total))
                {
                    MessageBox.Show($"Invalid or missing total in {Path.GetFileName(filePath)}. Skipping.", "Total Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMessage($"Invalid total in '{filePath}'.");
                    return;
                }

                var uniqueDesc = GetUniqueDescription(supplierName, reportPeriod);
                var idx = dataGridView1.Rows.Add(0, uniqueDesc, "", "", "", supplier.VATCode, total.ToString("0.00"), "", false, "Missing");
                ApplyLogicForRow(idx);

                CopyPdfToTargetFolder(filePath, uniqueDesc, total.ToString("0.00"));
                LogMessage($"Imported '{filePath}' as '{uniqueDesc}.pdf'.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing {Path.GetFileName(filePath)}: {ex.Message}. Skipping.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error in '{filePath}': {ex.Message}");
            }
        }



        private string ExtractTextFromPdf(string filePath)
        {
            var sb = new StringBuilder();
            using var document = PdfDocument.Open(filePath);
            foreach (var page in document.GetPages())
                sb.Append(ContentOrderTextExtractor.GetText(page));
            return sb.ToString();
        }

        private string DetectSupplier(string text)
        {
            foreach (var sup in SupplierConfigLoader.Suppliers)
                foreach (var kw in sup.DetectionKeywords)
                    if (text.Contains(kw, StringComparison.OrdinalIgnoreCase))
                        return sup.Name;
            return "Unknown Supplier";
        }

        private string ExtractTotalFromPdf(string text, Supplier supplier)
        {
            if (supplier.TotalExtractionRegexes != null)
            {
                foreach (var pat in supplier.TotalExtractionRegexes)
                {
                    var m = Regex.Match(text, pat);
                    if (m.Success)
                        return m.Groups[1].Value;
                }
            }
            return null;
        }

        private string GetUniqueDescription(string baseDescription, string reportPeriod)
        {
            var maxNum = 0;
            const string query = @"
                SELECT Description 
                  FROM CreditCardTransactions 
                 WHERE ReportPeriod = @ReportPeriod 
                   AND Description LIKE @Pattern";

            var parameters = new Dictionary<string, object>
            {
                { "@ReportPeriod", reportPeriod },
                { "@Pattern", baseDescription + "%" }
            };

            try
            {
                var dt = salesRepository.ExecuteSqlQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    var desc = row["Description"]?.ToString() ?? "";
                    var m = Regex.Match(desc, $@"^{Regex.Escape(baseDescription)} (\d+)$");
                    if (m.Success && int.TryParse(m.Groups[1].Value, out var n) && n > maxNum)
                        maxNum = n;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error fetching descriptions: {ex.Message}");
            }

            // also check in-memory rows
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var desc = row.Cells["Description"].Value?.ToString() ?? "";
                    var m = Regex.Match(desc, $@"^{Regex.Escape(baseDescription)} (\d+)$");
                    if (m.Success && int.TryParse(m.Groups[1].Value, out var n) && n > maxNum)
                        maxNum = n;
                }
            }

            return $"{baseDescription} {maxNum + 1}";
        }

        private void ApplyLogicForRow(int rowIndex)
        {
            if (rowIndex < 0) return;
            var row = dataGridView1.Rows[rowIndex];
            var desc = row.Cells["Description"].Value?.ToString();
            if (string.IsNullOrEmpty(desc)) return;

            var baseDesc = Regex.Replace(desc, @"\s+\d+$", "").Trim();
            var sup = SupplierConfigLoader.Suppliers.FirstOrDefault(s => s.Name.Equals(baseDesc, StringComparison.OrdinalIgnoreCase));
            if (sup != null)
            {
                row.Cells["GLAccount"].Value = sup.GLAccount;
                row.Cells["GLName"].Value = sup.GLName;
                row.Cells["VAT"].Value = sup.VATCode;
                row.Cells["Dimension"].Value = "0102";

                if (decimal.TryParse(row.Cells["Total"].Value?.ToString(), out var tot))
                {
                    var val = sup.VATCode == "I1" ? tot * 1.2m : tot;
                    row.Cells["Value"].Value = val.ToString("0.00");
                }

                row.Cells["IsModified"].Value = true;
            }
            else
            {
                row.Cells["GLAccount"].Value = "";
                row.Cells["GLName"].Value = "";
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = dataGridView1.Columns[e.ColumnIndex].Name;
            if (col == "VAT" || col == "Total")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (decimal.TryParse(row.Cells["Total"].Value?.ToString(), out var tot))
                {
                    var vat = row.Cells["VAT"].Value?.ToString();
                    var val = vat == "I1" ? tot * 1.2m : tot;
                    row.Cells["Value"].Value = val.ToString("0.00");
                }
                row.Cells["IsModified"].Value = true;
            }
        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void SaveDataToSqlServer(string reportPeriod)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                if (!int.TryParse(row.Cells["ID"].Value?.ToString(), out var id) || id == 0)
                {
                    var desc = row.Cells["Description"].Value?.ToString();
                    var gl = row.Cells["GLAccount"].Value?.ToString();
                    var dim = row.Cells["Dimension"].Value?.ToString();
                    var nm = row.Cells["GLName"].Value?.ToString();
                    var vat = row.Cells["VAT"].Value?.ToString();
                    if (!decimal.TryParse(row.Cells["Total"].Value?.ToString(), out var tot)) continue;

                    const string insert = @"
                        INSERT INTO CreditCardTransactions 
                            (Description, GLAccount, Dimension, GLName, VAT, Total, ReportPeriod)
                        VALUES 
                            (@Description, @GLAccount, @Dimension, @GLName, @VAT, @Total, @ReportPeriod);
                        SELECT CAST(scope_identity() AS int)";

                    var parameters = new Dictionary<string, object>
                    {
                        { "@Description",   desc },
                        { "@GLAccount",     gl  },
                        { "@Dimension",     dim },
                        { "@GLName",        nm  },
                        { "@VAT",           vat },
                        { "@Total",         tot },
                        { "@ReportPeriod",  reportPeriod }
                    };

                    try
                    {
                        var newId = salesRepository.ExecuteSqlScalar(insert, parameters);
                        row.Cells["ID"].Value = newId;
                        row.Cells["IsModified"].Value = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error inserting data: {ex.Message}", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogMessage($"Insert error: {ex.Message}");
                    }
                }
            }
        }

        private void LoadDataFromDatabase(string reportPeriod)
        {
            if (string.IsNullOrEmpty(reportPeriod))
            {
                MessageBox.Show("Please select a Report Period before loading data.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string query = @"
                SELECT ID, Description, GLAccount, Dimension, GLName, VAT, Total
                  FROM CreditCardTransactions
                 WHERE ReportPeriod = @ReportPeriod";

            var parameters = new Dictionary<string, object>
            {
                { "@ReportPeriod", reportPeriod }
            };

            try
            {
                var dt = salesRepository.ExecuteSqlQuery(query, parameters);
                dataGridView1.Rows.Clear();

                foreach (DataRow r in dt.Rows)
                {
                    var id = r["ID"] != DBNull.Value ? Convert.ToInt32(r["ID"]) : 0;
                    var desc = r["Description"]?.ToString() ?? "";
                    var gl = r["GLAccount"]?.ToString() ?? "";
                    var dim = r["Dimension"]?.ToString() ?? "";
                    var nm = r["GLName"]?.ToString() ?? "";
                    var vat = r["VAT"]?.ToString() ?? "";
                    var tot = r["Total"] != DBNull.Value ? Convert.ToDecimal(r["Total"]) : 0m;
                    var val = vat == "I1" ? tot * 1.2m : tot;

                    dataGridView1.Rows.Add(
                        id, desc, gl, dim, nm, vat,
                        tot.ToString("0.00"),
                        val.ToString("0.00"),
                        false, "Missing"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data:\n{ex.Message}", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Load error: {ex.Message}");
            }

            UpdateInvoiceStatus();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var rp = ComboMonth.SelectedItem?.ToString();
            LoadDataFromDatabase(rp);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var rp = ComboMonth.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(rp))
            {
                MessageBox.Show("Please select a Report Period before updating.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to update all records?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            const string update = @"
                UPDATE CreditCardTransactions
                   SET Description = @Description,
                       GLAccount   = @GLAccount,
                       Dimension   = @Dimension,
                       GLName      = @GLName,
                       VAT         = @VAT,
                       Total       = @Total
                 WHERE ID = @ID";

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (!int.TryParse(row.Cells["ID"].Value?.ToString(), out var id)) continue;

                var desc = row.Cells["Description"].Value?.ToString();
                var gl = row.Cells["GLAccount"].Value?.ToString();
                var dim = row.Cells["Dimension"].Value?.ToString();
                var nm = row.Cells["GLName"].Value?.ToString();
                var vat = row.Cells["VAT"].Value?.ToString();
                if (!decimal.TryParse(row.Cells["Total"].Value?.ToString(), out var tot)) continue;

                var parameters = new Dictionary<string, object>
                {
                    { "@Description", desc },
                    { "@GLAccount",   gl   },
                    { "@Dimension",   dim  },
                    { "@GLName",      nm   },
                    { "@VAT",         vat  },
                    { "@Total",       tot  },
                    { "@ID",          id   }
                };

                try
                {
                    var affected = salesRepository.ExecuteSqlNonQuery(update, parameters);
                    if (affected > 0)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        row.Cells["IsModified"].Value = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating ID {id}: {ex.Message}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessage($"Update error ID {id}: {ex.Message}");
                }
            }

            LoadDataFromDatabase(rp);
        }

        private bool CheckInvoiceExists(string description, string total)
        {
            var folder = GetInvoiceFolderPath();
            if (string.IsNullOrEmpty(folder)) return false;

            var sup = SupplierConfigLoader.Suppliers.FirstOrDefault(s => description.StartsWith(s.Name, StringComparison.OrdinalIgnoreCase));
            if (sup == null) return false;

            var pattern = sup.FilenamePattern ?? "{Description}.pdf";
            var d = SanitizeFileName(description);
            var t = SanitizeFileName(total);
            var expected = pattern.Replace("{Description}", d).Replace("{Number}", t);
            var path = Path.Combine(folder, expected);

            return File.Exists(path) ||
                   Directory.EnumerateFiles(folder, "*.pdf")
                            .Any(f => Path.GetFileNameWithoutExtension(f)
                                      .StartsWith($"{d} {t} ", StringComparison.OrdinalIgnoreCase));
        }

        private void UpdateInvoiceStatus()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                var desc = row.Cells["Description"].Value?.ToString();
                var tot = row.Cells["Total"].Value?.ToString();

                if (string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(tot))
                {
                    row.Cells["Invoice"].Value = "Missing";
                    row.Cells["Invoice"].Style.BackColor = System.Drawing.Color.Tomato;
                    continue;
                }

                var exists = CheckInvoiceExists(desc, tot);
                row.Cells["Invoice"].Value = exists ? "Exists" : "Missing";
                row.Cells["Invoice"].Style.BackColor = exists ? System.Drawing.Color.YellowGreen : System.Drawing.Color.Tomato;
            }
        }

        private void CopyPdfToTargetFolder(string sourceFilePath, string description, string total)
        {
            var targetFolder = GetInvoiceFolderPath();
            if (string.IsNullOrEmpty(targetFolder)) return;
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var sup = SupplierConfigLoader.Suppliers.FirstOrDefault(s => description.StartsWith(s.Name, StringComparison.OrdinalIgnoreCase));
            var pattern = sup?.FilenamePattern ?? "{Description}.pdf";
            var d = SanitizeFileName(description);
            var t = SanitizeFileName(total);

            var fileName = pattern.Replace("{Description}", d).Replace("{Number}", t);
            if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                fileName += ".pdf";

            var dest = Path.Combine(targetFolder, fileName);
            if (File.Exists(dest))
                dest = Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(fileName)
                                           + " " + DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                           + ".pdf");

            try
            {
                File.Copy(sourceFilePath, dest);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to '{dest}': {ex.Message}");
                LogMessage($"Copy failed: {ex.Message}");
            }
        }

        private void LogMessage(string message)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportLog.txt");
            try
            {
                File.AppendAllText(path, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch { /* ignore */ }
        }

        private string GetInvoiceFolderPath()
        {
            var path = ConfigurationManager.AppSettings["InvoiceFolderPath"];
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Invoice folder path is not configured. Please set 'InvoiceFolderPath' in app.config.",
                                "Configuration Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage("InvoiceFolderPath not set in app.config.");
            }
            return path;
        }

        private string SanitizeFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');
            return input;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Get the selected period
            var period = ComboMonth.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(period))
            {
                MessageBox.Show(
                    "Please select a Report Period before clearing.",
                    "Selection Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 2. Confirm the operation
            var answer = MessageBox.Show(
                $"This will permanently delete all records for \"{period}\".\nAre you sure?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                // 3. Perform the delete
                const string sql = @"
            DELETE FROM CreditCardTransactions
             WHERE ReportPeriod = @ReportPeriod";
                var parameters = new Dictionary<string, object>
        {
            { "@ReportPeriod", period }
        };
                salesRepository.ExecuteSqlNonQuery(sql, parameters);

                LogMessage($"Deleted all CreditCardTransactions for period '{period}'.");

                // 4. Refresh UI
                PopulateReportPeriodComboBox();    // in case you want to remove that period
                LoadDataFromDatabase(period);      // will now show an empty grid
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error deleting records for {period}:\n{ex.Message}",
                    "Delete Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LogMessage($"Error deleting period '{period}': {ex}");
            }
        }
    }
}
