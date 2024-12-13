using BG_Menu.Helpers; // Ensure this namespace is included
using BG_Menu.Models;  // Ensure this namespace is included
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization; // For CultureInfo
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class NewCreditCard : Form
    {
        private ContextMenuStrip loadButtonContextMenu;

        public NewCreditCard()
        {
            InitializeComponent();
            InitializeDataGridView();

            // Load supplier configurations from the database
            SupplierConfigLoader.LoadSuppliers();

            InitializeContextMenu();

            // Subscribe to DataGridView events for automatic Value calculation and tracking changes
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;

            // Populate the ComboMonth ComboBox with unique ReportPeriods and the current working month
            PopulateReportPeriodComboBox();
        }
        private void InitializeContextMenu()
        {
            loadButtonContextMenu = new ContextMenuStrip();
            ToolStripMenuItem reloadSuppliersItem = new ToolStripMenuItem("Reload Suppliers");
            reloadSuppliersItem.Click += ReloadSuppliersItem_Click;
            loadButtonContextMenu.Items.Add(reloadSuppliersItem);

            // Associate the context menu with the Load button
            btnLoad.ContextMenuStrip = loadButtonContextMenu;
        }

        private void ReloadSuppliersItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Reload suppliers from the database
                SupplierConfigLoader.LoadSuppliers();

                // Optionally, refresh any UI elements that depend on supplier data
                // For example, re-apply logic to existing rows if necessary
                RefreshSupplierDependentLogic();

                MessageBox.Show("Suppliers successfully reloaded.", "Reload Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                string description = row.Cells["Description"].Value?.ToString();
                string vatCode = row.Cells["VAT"].Value?.ToString();
                string totalStr = row.Cells["Total"].Value?.ToString();

                if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(totalStr))
                {
                    // Skip rows with incomplete data
                    continue;
                }

                // Re-apply logic based on the updated supplier configurations
                ApplyLogicForRow(row.Index);
            }
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();

            // 0. ID (Hidden Column)
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                Visible = false // Hide the ID column
            };
            dataGridView1.Columns.Add(idColumn);

            // 1. Description
            dataGridView1.Columns.Add("Description", "Description");

            // 2. GL Account
            dataGridView1.Columns.Add("GLAccount", "GL Account");

            // 3. Dimension
            dataGridView1.Columns.Add("Dimension", "Dimension");

            // 4. GL Name
            dataGridView1.Columns.Add("GLName", "GL Name");

            // 5. VAT (ComboBox with I1 and I2)
            DataGridViewComboBoxColumn vatColumn = new DataGridViewComboBoxColumn
            {
                Name = "VAT",
                HeaderText = "VAT",
                Items = { "I1", "I2" },
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
            };
            dataGridView1.Columns.Add(vatColumn);

            // 6. Total
            dataGridView1.Columns.Add("Total", "Total");

            // 7. Value
            DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Value",
                ReadOnly = true // Make it read-only since it's calculated
            };
            dataGridView1.Columns.Add(valueColumn);

            // 8. IsModified (Hidden Column for tracking changes)
            DataGridViewCheckBoxColumn isModifiedColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsModified",
                HeaderText = "IsModified",
                Visible = false // Hide the column
            };
            dataGridView1.Columns.Add(isModifiedColumn);

            // 9. Invoice
            DataGridViewTextBoxColumn invoiceColumn = new DataGridViewTextBoxColumn
            {
                Name = "Invoice",
                HeaderText = "Invoice",
                ReadOnly = true // Read-only since it's a status field
            };
            dataGridView1.Columns.Add(invoiceColumn);
        }

        /// <summary>
        /// Populates the ComboMonth ComboBox with unique ReportPeriod values from the database and the current working month.
        /// </summary>
        private void PopulateReportPeriodComboBox()
        {
            // Clear existing items
            ComboMonth.Items.Clear();

            // Get unique ReportPeriod values from the database
            List<string> reportPeriods = GetUniqueReportPeriodsFromDatabase();

            // Calculate the current working month based on the current date
            string currentWorkingMonth = CalculateCurrentWorkingMonth();

            // Add the working month to the list if it's not already present
            if (!reportPeriods.Contains(currentWorkingMonth))
            {
                reportPeriods.Add(currentWorkingMonth);
            }

            // Sort the report periods in descending order (most recent first)
            reportPeriods = reportPeriods
                .Select(rp => new { Period = rp, Date = DateTime.ParseExact(rp, "MMMM yyyy", CultureInfo.InvariantCulture) })
                .OrderByDescending(rp => rp.Date)
                .Select(rp => rp.Period)
                .ToList();

            // Add the sorted report periods to the ComboMonth ComboBox
            ComboMonth.Items.AddRange(reportPeriods.ToArray());

            // Set the selected item to the current working month
            ComboMonth.SelectedItem = currentWorkingMonth;
        }

        /// <summary>
        /// Retrieves a list of unique ReportPeriod values from the CreditCardTransactions table.
        /// </summary>
        /// <returns>List of unique ReportPeriod strings.</returns>
        private List<string> GetUniqueReportPeriodsFromDatabase()
        {
            List<string> reportPeriods = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            string query = "SELECT DISTINCT ReportPeriod FROM CreditCardTransactions";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string reportPeriod = reader["ReportPeriod"]?.ToString();
                            if (!string.IsNullOrEmpty(reportPeriod))
                            {
                                reportPeriods.Add(reportPeriod);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving Report Periods from SQL Server:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error retrieving Report Periods: {ex.Message}");
            }

            return reportPeriods;
        }

        /// <summary>
        /// Calculates the current working month based on today's date.
        /// If today is the 10th or later, the working month is the next month; otherwise, it's the current month.
        /// </summary>
        /// <returns>Formatted working month string (e.g., "January 2025").</returns>
        private string CalculateCurrentWorkingMonth()
        {
            DateTime today = DateTime.Today;
            DateTime workingMonthDate;

            if (today.Day >= 10)
            {
                // Move to the first day of the next month
                workingMonthDate = today.AddMonths(1);
            }
            else
            {
                // Current month
                workingMonthDate = today;
            }

            // Format as "MMMM yyyy" (e.g., "January 2025")
            return workingMonthDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Retrieves the invoice folder path from app.config.
        /// </summary>
        /// <returns>Network path as a string.</returns>
        private string GetInvoiceFolderPath()
        {
            string path = ConfigurationManager.AppSettings["InvoiceFolderPath"];
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Invoice folder path is not configured. Please set 'InvoiceFolderPath' in app.config.", "Configuration Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage("InvoiceFolderPath not set in app.config.");
            }
            return path;
        }

        /// <summary>
        /// Copies the PDF file to the target directory with a standardized naming convention.
        /// </summary>
        /// <param name="sourceFilePath">Original PDF file path.</param>
        /// <param name="description">Description extracted from the PDF.</param>
        /// <param name="total">Total amount extracted from the PDF.</param>
        private void CopyPdfToTargetFolder(string sourceFilePath, string description, string total)
        {
            string targetFolder = GetInvoiceFolderPath();
            if (string.IsNullOrEmpty(targetFolder))
            {
                // Path is not configured; exit the method.
                return;
            }

            // Ensure the target directory exists; if not, create it.
            if (!Directory.Exists(targetFolder))
            {
                try
                {
                    Directory.CreateDirectory(targetFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create target directory '{targetFolder}': {ex.Message}", "Directory Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessage($"Failed to create directory '{targetFolder}': {ex.Message}");
                    return;
                }
            }

            // Find the supplier based on the description
            var supplier = SupplierConfigLoader.Suppliers.FirstOrDefault(s =>
                description.StartsWith(s.Name, StringComparison.OrdinalIgnoreCase));

            // Define the filename pattern from supplier or use default
            string filenamePattern = supplier?.FilenamePattern ?? "{Description}.pdf";

            // Sanitize description and total to create a valid filename.
            string sanitizedDescription = SanitizeFileName(description);
            string sanitizedTotal = SanitizeFileName(total);

            // Replace placeholders in the pattern
            string baseFileName = filenamePattern
                .Replace("{Description}", sanitizedDescription)
                .Replace("{Number}", sanitizedTotal);

            // Truncate the filename if it's too long to prevent exceeding path length limits.
            int maxFileNameLength = 200; // Adjust as needed.
            if (baseFileName.Length > maxFileNameLength)
            {
                baseFileName = baseFileName.Substring(0, maxFileNameLength);
            }

            // Construct the new filename.
            string newFileName = baseFileName;
            if (!newFileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                newFileName += ".pdf";
            }
            string destinationFilePath = Path.Combine(targetFolder, newFileName);

            // Handle filename conflicts by appending a timestamp.
            if (File.Exists(destinationFilePath) || IsReservedFileName(baseFileName))
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName = $"{baseFileName} {timestamp}.pdf";
                destinationFilePath = Path.Combine(targetFolder, newFileName);
            }

            try
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy file to '{destinationFilePath}': {ex.Message}", "File Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Failed to copy '{sourceFilePath}' to '{destinationFilePath}': {ex.Message}");
            }
        }

        /// <summary>
        /// Removes invalid characters from filenames.
        /// </summary>
        /// <param name="input">Original filename string.</param>
        /// <returns>Sanitized filename string.</returns>
        private string SanitizeFileName(string input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input;
        }

        /// <summary>
        /// Checks if the filename is a reserved Windows name.
        /// </summary>
        private bool IsReservedFileName(string fileName)
        {
            string[] reservedNames = { "CON", "PRN", "AUX", "NUL",
                                        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                                        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName).ToUpper();
            return reservedNames.Contains(nameWithoutExtension);
        }

        /// <summary>
        /// Logs messages to a text file for auditing and troubleshooting.
        /// </summary>
        /// <param name="message">Message to log.</param>
        private void LogMessage(string message)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportLog.txt");
            try
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch
            {
                // If logging fails, silently ignore to prevent user disruption.
            }
        }

        /// <summary>
        /// Handles the Import button click event.
        /// </summary>
        private void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                openFileDialog.Title = "Select Invoice PDF Files";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string department = ComboDepartment.SelectedItem?.ToString();
                    string reportPeriod = ComboMonth.SelectedItem?.ToString();

                    if (string.IsNullOrEmpty(department) || string.IsNullOrEmpty(reportPeriod))
                    {
                        MessageBox.Show("Please select both Department and Report Period before importing.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (var selectedPdfFilePath in openFileDialog.FileNames)
                    {
                        ExtractDataFromPdfAndPopulate(selectedPdfFilePath, department, reportPeriod);
                    }

                    // Save all new data to SQL Server after processing
                    SaveDataToSqlServer(department, reportPeriod);

                    // Refresh the ComboMonth ComboBox to include any new ReportPeriods
                    PopulateReportPeriodComboBox();

                    // Reload the data to reflect the latest entries without duplicates
                    LoadDataFromDatabase(department, reportPeriod);
                }
            }
        }

        /// <summary>
        /// Extracts data from a PDF file and populates the DataGridView.
        /// Also copies the PDF to the target directory with a standardized name.
        /// </summary>
        private void ExtractDataFromPdfAndPopulate(string filePath, string department, string reportPeriod)
        {
            try
            {
                string extractedText = ExtractTextFromPdf(filePath);

                // Detect supplier
                string supplierName = DetectSupplier(extractedText);
                if (supplierName == "Unknown Supplier")
                {
                    MessageBox.Show($"Could not identify a known supplier from the invoice: {Path.GetFileName(filePath)}. Skipping this file.", "Unknown Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMessage($"Unknown supplier in file '{Path.GetFileName(filePath)}'. File skipped.");
                    return;
                }

                // Find supplier configuration
                var supplier = SupplierConfigLoader.Suppliers.FirstOrDefault(s => s.Name.Equals(supplierName, StringComparison.OrdinalIgnoreCase));
                if (supplier == null)
                {
                    MessageBox.Show($"Supplier configuration for '{supplierName}' not found. Skipping this file.", "Configuration Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMessage($"Supplier configuration for '{supplierName}' not found. File '{Path.GetFileName(filePath)}' skipped.");
                    return;
                }

                // Extract total
                string total = ExtractTotalFromPdf(extractedText, supplier);
                if (string.IsNullOrEmpty(total))
                {
                    MessageBox.Show($"Unable to find the total value for supplier '{supplierName}' in the PDF: {Path.GetFileName(filePath)}. Skipping this file.", "Total Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMessage($"Total not found in file '{Path.GetFileName(filePath)}'. File skipped.");
                    return;
                }

                // Validate total is a decimal
                if (!decimal.TryParse(total, out decimal totalValue))
                {
                    MessageBox.Show($"Invalid total value '{total}' in file: {Path.GetFileName(filePath)}. Skipping this file.", "Invalid Total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMessage($"Invalid total '{total}' in file '{Path.GetFileName(filePath)}'. File skipped.");
                    return;
                }

                // Generate a unique description
                string uniqueDescription = GetUniqueDescription(supplierName, department, reportPeriod);

                // Add the row to the DataGridView with ID = 0 (indicating a new record)
                int rowIndex = dataGridView1.Rows.Add(0, uniqueDescription, "", "", "", supplier.VATCode, totalValue.ToString("0.00"), "", false, "Missing"); // VAT from supplier config, IsModified = false, Invoice initial as "Missing"

                // Apply logic to the new row
                ApplyLogicForRow(rowIndex);

                // Copy the PDF to the target directory with the new naming convention
                CopyPdfToTargetFolder(filePath, uniqueDescription, totalValue.ToString("0.00"));
                LogMessage($"File '{Path.GetFileName(filePath)}' imported and copied as '{uniqueDescription}.pdf'.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading or processing PDF '{Path.GetFileName(filePath)}': {ex.Message}. Skipping this file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error processing file '{Path.GetFileName(filePath)}': {ex.Message}. File skipped.");
            }
        }

        /// <summary>
        /// Extracts text from a PDF file using PdfPig.
        /// </summary>
        private string ExtractTextFromPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    // Use PdfPig's content extraction method for better accuracy
                    text.Append(ContentOrderTextExtractor.GetText(page));
                }
            }

            return text.ToString();
        }

        /// <summary>
        /// Detects the supplier from the extracted text using loaded supplier mappings.
        /// </summary>
        private string DetectSupplier(string extractedText)
        {
            foreach (var supplier in SupplierConfigLoader.Suppliers)
            {
                foreach (var keyword in supplier.DetectionKeywords)
                {
                    if (extractedText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        return supplier.Name;
                    }
                }
            }

            return "Unknown Supplier";
        }

        /// <summary>
        /// Extracts the total amount from the PDF based on the supplier.
        /// </summary>
        private string ExtractTotalFromPdf(string extractedText, Supplier supplier)
        {
            if (supplier.TotalExtractionRegexes != null && supplier.TotalExtractionRegexes.Count > 0)
            {
                foreach (var regexPattern in supplier.TotalExtractionRegexes)
                {
                    var match = Regex.Match(extractedText, regexPattern);
                    if (match.Success)
                    {
                        string total = match.Groups[1].Value;

                        // Remove commas if present
                        return total.Replace(",", "");
                    }
                }
            }

            return null; // Return null if no matches are found
        }

        /// <summary>
        /// Generates a unique description by appending a sequential number based on existing entries.
        /// </summary>
        /// <param name="baseDescription">Base description (e.g., "Amazon").</param>
        /// <param name="department">Selected Department.</param>
        /// <param name="reportPeriod">Selected Report Period.</param>
        /// <returns>Unique description string (e.g., "Amazon 3").</returns>
        private string GetUniqueDescription(string baseDescription, string department, string reportPeriod)
        {
            int maxNumber = 0;
            string query = @"SELECT Description FROM CreditCardTransactions 
                             WHERE Department = @Department 
                             AND ReportPeriod = @ReportPeriod 
                             AND Description LIKE @DescriptionPattern";

            // Find the supplier to get the description pattern
            var supplier = SupplierConfigLoader.Suppliers.FirstOrDefault(s =>
                s.Name.Equals(baseDescription, StringComparison.OrdinalIgnoreCase));

            string descriptionPattern = supplier != null && !string.IsNullOrEmpty(supplier.DescriptionPattern)
                ? supplier.DescriptionPattern.Replace("{SupplierName}", baseDescription).Replace("{Number}", "%")
                : baseDescription + " %";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Department", department);
                    cmd.Parameters.AddWithValue("@ReportPeriod", reportPeriod);
                    cmd.Parameters.AddWithValue("@DescriptionPattern", descriptionPattern);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string existingDescription = reader["Description"]?.ToString();
                            if (existingDescription != null)
                            {
                                // Extract the number from descriptions like "Amazon 1"
                                var match = Regex.Match(existingDescription, $@"^{Regex.Escape(baseDescription)} (\d+)$");
                                if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
                                {
                                    if (number > maxNumber)
                                        maxNumber = number;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving existing descriptions from SQL Server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error retrieving descriptions for '{baseDescription}' in '{department}' during '{reportPeriod}': {ex.Message}");
            }

            // Check the DataGridView for any in-memory entries
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["Description"].Value != null)
                {
                    string existingDescription = row.Cells["Description"].Value.ToString();
                    var match = Regex.Match(existingDescription, $@"^{Regex.Escape(baseDescription)} (\d+)$");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
                    {
                        if (number > maxNumber)
                            maxNumber = number;
                    }
                }
            }

            // Generate the next unique number
            int nextNumber = maxNumber + 1;

            // Use the supplier's description pattern if available
            if (supplier != null && !string.IsNullOrEmpty(supplier.DescriptionPattern))
            {
                string uniqueDescription = supplier.DescriptionPattern
                    .Replace("{SupplierName}", baseDescription)
                    .Replace("{Number}", nextNumber.ToString());

                return uniqueDescription;
            }
            else
            {
                return $"{baseDescription} {nextNumber}";
            }
        }

        /// <summary>
        /// Applies business logic to a newly added row based on its description.
        /// </summary>
        private void ApplyLogicForRow(int rowIndex)
        {
            if (rowIndex < 0)
                return;

            DataGridViewRow row = dataGridView1.Rows[rowIndex];
            string description = row.Cells["Description"].Value?.ToString();

            if (!string.IsNullOrEmpty(description))
            {
                // Extract the base description by removing any trailing numbers and spaces
                string baseDescription = GetBaseDescription(description);

                // Find the supplier based on the base description
                var supplier = SupplierConfigLoader.Suppliers.FirstOrDefault(s =>
                    s.Name.Equals(baseDescription, StringComparison.OrdinalIgnoreCase));

                if (supplier != null)
                {
                    // Auto-populate fields based on supplier configuration
                    row.Cells["GLAccount"].Value = supplier.GLAccount;
                    row.Cells["GLName"].Value = supplier.GLName;
                    row.Cells["VAT"].Value = supplier.VATCode;
                    row.Cells["Dimension"].Value = "0102"; // Assuming Dimension is consistent

                    // Recalculate Value based on VAT and Total
                    string vatCode = supplier.VATCode;
                    if (decimal.TryParse(row.Cells["Total"].Value?.ToString(), out decimal total))
                    {
                        decimal calculatedValue = vatCode == "I1" ? total * 1.2m : total * 1.0m;
                        row.Cells["Value"].Value = calculatedValue.ToString("0.00");
                    }

                    // Mark the row as modified for potential updates
                    row.Cells["IsModified"].Value = true;
                }
                else
                {
                    // Handle unknown suppliers if necessary
                    row.Cells["GLAccount"].Value = "";
                    row.Cells["GLName"].Value = "";
                    // Optionally, set default VAT or leave it as is
                }
            }
        }

        /// <summary>
        /// Extracts the base description by removing any trailing numbers and spaces.
        /// </summary>
        private string GetBaseDescription(string description)
        {
            // Use regex to remove trailing numbers and spaces from the description
            return Regex.Replace(description, @"\s+\d+$", "").Trim();
        }

        /// <summary>
        /// Handles the CellValueChanged event to recalculate the Value column when VAT or Total changes and mark the row as modified.
        /// </summary>
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the change is in the VAT or Total column
            if (e.RowIndex >= 0 &&
                (dataGridView1.Columns[e.ColumnIndex].Name == "VAT" ||
                 dataGridView1.Columns[e.ColumnIndex].Name == "Total"))
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string vatCode = row.Cells["VAT"].Value?.ToString();
                string totalStr = row.Cells["Total"].Value?.ToString();

                if (decimal.TryParse(totalStr, out decimal total))
                {
                    decimal calculatedValue = vatCode == "I1" ? total * 1.2m : total * 1.0m;
                    row.Cells["Value"].Value = calculatedValue.ToString("0.00");
                }

                // Mark the row as modified
                row.Cells["IsModified"].Value = true;
            }
        }

        /// <summary>
        /// Handles the CurrentCellDirtyStateChanged event to commit edits immediately.
        /// This ensures that the CellValueChanged event is fired when a ComboBox selection changes.
        /// </summary>
        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Saves the new data from the DataGridView to the SQL Server database.
        /// Only inserts rows where ID = 0, indicating new records.
        /// After insertion, updates the DataGridView row with the generated ID.
        /// </summary>
        private void SaveDataToSqlServer(string department, string reportPeriod)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Retrieve the ID; skip if ID is missing or not zero
                    if (!int.TryParse(row.Cells["ID"].Value?.ToString(), out int id) || id != 0)
                    {
                        // Existing row; skip insertion to prevent duplicates
                        continue;
                    }

                    // Retrieve cell values
                    string description = row.Cells["Description"].Value?.ToString();
                    string glAccount = row.Cells["GLAccount"].Value?.ToString();
                    string dimension = row.Cells["Dimension"].Value?.ToString();
                    string glName = row.Cells["GLName"].Value?.ToString();
                    string vat = row.Cells["VAT"].Value?.ToString();
                    string total = row.Cells["Total"].Value?.ToString();

                    // Skip rows with incomplete data
                    if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(glAccount) ||
                        string.IsNullOrEmpty(dimension) || string.IsNullOrEmpty(glName) ||
                        string.IsNullOrEmpty(vat) || string.IsNullOrEmpty(total) ||
                        string.IsNullOrEmpty(department) ||
                        string.IsNullOrEmpty(reportPeriod))
                    {
                        continue;
                    }

                    // Prepare the INSERT command and retrieve the inserted ID using SCOPE_IDENTITY()
                    string insertQuery = @"INSERT INTO CreditCardTransactions 
                                           (Description, GLAccount, Dimension, GLName, VAT, Total, Department, ReportPeriod)
                                           VALUES (@Description, @GLAccount, @Dimension, @GLName, @VAT, @Total, @Department, @ReportPeriod);
                                           SELECT CAST(scope_identity() AS int)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@GLAccount", glAccount);
                        cmd.Parameters.AddWithValue("@Dimension", dimension);
                        cmd.Parameters.AddWithValue("@GLName", glName);
                        cmd.Parameters.AddWithValue("@VAT", vat);
                        cmd.Parameters.AddWithValue("@Total", decimal.Parse(total)); // Ensure 'Total' is a decimal
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@ReportPeriod", reportPeriod);

                        try
                        {
                            // Execute the INSERT command and retrieve the new ID
                            int newId = (int)cmd.ExecuteScalar();

                            // Update the DataGridView row with the new ID
                            row.Cells["ID"].Value = newId;

                            // Optionally, mark the row as not modified
                            row.Cells["IsModified"].Value = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error inserting data into SQL Server: {ex.Message}", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMessage($"Error inserting record '{description}': {ex.Message}");
                        }
                    }
                }

                conn.Close();
            }

            MessageBox.Show("Data successfully imported and saved to SQL Server.", "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Loads data from the SQL Server database based on the selected Department and Report Period.
        /// This method is called by both the Load button and after Import/Update operations.
        /// </summary>
        private void LoadDataFromDatabase(string department, string reportPeriod)
        {
            // Validate selections
            if (string.IsNullOrEmpty(department) || string.IsNullOrEmpty(reportPeriod))
            {
                MessageBox.Show("Please select both Department and Report Period before loading data.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrieve the connection string from configuration
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Define the SELECT query with parameters, including ID
                    string selectQuery = @"
                        SELECT ID, Description, GLAccount, Dimension, GLName, VAT, Total 
                        FROM CreditCardTransactions 
                        WHERE Department = @Department AND ReportPeriod = @ReportPeriod";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@ReportPeriod", reportPeriod);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows in the DataGridView
                            dataGridView1.Rows.Clear();

                            // Read each record and add it to the DataGridView
                            while (reader.Read())
                            {
                                // Retrieve values from the reader
                                int id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0;
                                string description = reader["Description"]?.ToString() ?? string.Empty;
                                string glAccount = reader["GLAccount"]?.ToString() ?? string.Empty;
                                string dimension = reader["Dimension"]?.ToString() ?? string.Empty;
                                string glName = reader["GLName"]?.ToString() ?? string.Empty;
                                string vat = reader["VAT"]?.ToString() ?? string.Empty;
                                decimal total = reader["Total"] != DBNull.Value ? Convert.ToDecimal(reader["Total"]) : 0m;

                                // Calculate the Value based on VAT
                                decimal value = vat == "I1" ? total * 1.2m : total * 1.0m;

                                // Add the row to the DataGridView, including the ID and IsModified flag set to false
                                dataGridView1.Rows.Add(
                                    id, // ID
                                    description,
                                    glAccount,
                                    dimension,
                                    glName,
                                    vat,
                                    total.ToString("0.00"),
                                    value.ToString("0.00"),
                                    false, // IsModified flag set to false
                                    "Missing" // Initial value for Invoice column
                                );
                            }
                        }
                    }

                    conn.Close();
                }

                // Update the Invoice status after loading data
                UpdateInvoiceStatus();

                MessageBox.Show("Data successfully loaded from SQL Server.", "Load Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the database operations
                MessageBox.Show($"Error loading data from SQL Server:\n{ex.Message}", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error loading data: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the Load button click event to load saved data from SQL Server based on selected Department and Report Period.
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Retrieve selected Department and Report Period
            string department = ComboDepartment.SelectedItem?.ToString();
            string reportPeriod = ComboMonth.SelectedItem?.ToString();

            // Call the centralized data loading method
            LoadDataFromDatabase(department, reportPeriod);
        }

        /// <summary>
        /// Handles the Update button click event to update all rows in the SQL Server database.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Retrieve selected Department and Report Period
            string department = ComboDepartment.SelectedItem?.ToString();
            string reportPeriod = ComboMonth.SelectedItem?.ToString();

            // Validate selections
            if (string.IsNullOrEmpty(department) || string.IsNullOrEmpty(reportPeriod))
            {
                MessageBox.Show("Please select both Department and Report Period before updating data.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm update operation
            var confirmResult = MessageBox.Show("Are you sure you want to update all records?",
                                                "Confirm Update",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            // Retrieve the connection string from configuration
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        // Retrieve the ID; skip if ID is missing or invalid
                        if (row.Cells["ID"].Value == null || !int.TryParse(row.Cells["ID"].Value.ToString(), out int id))
                        {
                            continue;
                        }

                        // Retrieve cell values
                        string description = row.Cells["Description"].Value?.ToString();
                        string glAccount = row.Cells["GLAccount"].Value?.ToString();
                        string dimension = row.Cells["Dimension"].Value?.ToString();
                        string glName = row.Cells["GLName"].Value?.ToString();
                        string vat = row.Cells["VAT"].Value?.ToString();
                        string totalStr = row.Cells["Total"].Value?.ToString();

                        // Validate required fields
                        if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(glAccount) ||
                            string.IsNullOrEmpty(dimension) || string.IsNullOrEmpty(glName) ||
                            string.IsNullOrEmpty(vat) || string.IsNullOrEmpty(totalStr))
                        {
                            // Optionally, you can notify the user about incomplete rows
                            continue;
                        }

                        // Parse Total to decimal
                        if (!decimal.TryParse(totalStr, out decimal total))
                        {
                            // Optionally, notify the user about invalid total values
                            continue;
                        }

                        // Prepare the UPDATE command
                        string updateQuery = @"
                            UPDATE CreditCardTransactions
                            SET Description = @Description,
                                GLAccount = @GLAccount,
                                Dimension = @Dimension,
                                GLName = @GLName,
                                VAT = @VAT,
                                Total = @Total
                            WHERE ID = @ID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@GLAccount", glAccount);
                            cmd.Parameters.AddWithValue("@Dimension", dimension);
                            cmd.Parameters.AddWithValue("@GLName", glName);
                            cmd.Parameters.AddWithValue("@VAT", vat);
                            cmd.Parameters.AddWithValue("@Total", total);
                            cmd.Parameters.AddWithValue("@ID", id);

                            try
                            {
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    // Optionally, mark the row as updated (e.g., change background color)
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                                    // Reset the IsModified flag
                                    row.Cells["IsModified"].Value = false;
                                }
                                else
                                {
                                    // Optionally, notify the user if no rows were updated
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error updating record ID {id}: {ex.Message}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LogMessage($"Error updating record ID {id}: {ex.Message}");
                            }
                        }
                    }

                    conn.Close();
                }

                MessageBox.Show("All records have been successfully updated.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload the data to reflect the latest updates
                LoadDataFromDatabase(department, reportPeriod);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the database operations
                MessageBox.Show($"Error updating data in SQL Server:\n{ex.Message}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error updating data: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the invoice PDF exists in the Invoice folder based on Description and Total.
        /// </summary>
        /// <param name="description">Description from the DataGridView row.</param>
        /// <param name="total">Total from the DataGridView row.</param>
        /// <returns>True if the invoice exists; otherwise, false.</returns>
        private bool CheckInvoiceExists(string description, string total)
        {
            string targetFolder = GetInvoiceFolderPath();
            if (string.IsNullOrEmpty(targetFolder))
            {
                // Path is not configured; consider invoice as missing
                return false;
            }

            // Find the supplier based on the description
            var supplier = SupplierConfigLoader.Suppliers.FirstOrDefault(s =>
                description.StartsWith(s.Name, StringComparison.OrdinalIgnoreCase));

            if (supplier == null)
            {
                // Supplier not found; consider invoice as missing
                return false;
            }

            // Construct the expected filename using supplier's filename pattern
            string filenamePattern = supplier.FilenamePattern ?? "{Description}.pdf";
            string sanitizedDescription = SanitizeFileName(description);
            string sanitizedTotal = SanitizeFileName(total);
            string expectedFileName = filenamePattern
                .Replace("{Description}", sanitizedDescription)
                .Replace("{Number}", sanitizedTotal);

            // Check if exact filename exists
            string exactFilePath = Path.Combine(targetFolder, expectedFileName);
            bool exists = File.Exists(exactFilePath);

            // If not found, check for files with timestamps or variations
            if (!exists)
            {
                exists = Directory.EnumerateFiles(targetFolder, "*.pdf")
                    .Any(file => Path.GetFileNameWithoutExtension(file)
                        .StartsWith($"{sanitizedDescription} {sanitizedTotal} ", StringComparison.OrdinalIgnoreCase));
            }

            return exists;
        }

        /// <summary>
        /// Updates the Invoice column status for all rows in the DataGridView.
        /// Sets the background color to YellowGreen if the invoice exists, otherwise Tomato.
        /// </summary>
        private void UpdateInvoiceStatus()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string description = row.Cells["Description"].Value?.ToString();
                string total = row.Cells["Total"].Value?.ToString();

                if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(total))
                {
                    // If description or total is missing, mark as missing
                    row.Cells["Invoice"].Value = "Missing";
                    row.Cells["Invoice"].Style.BackColor = System.Drawing.Color.Tomato;
                    continue;
                }

                // Check if the invoice exists based on the description and total
                bool exists = CheckInvoiceExists(description, total);
                row.Cells["Invoice"].Value = exists ? "Exists" : "Missing";
                row.Cells["Invoice"].Style.BackColor = exists ? System.Drawing.Color.YellowGreen : System.Drawing.Color.Tomato;
            }
        }
    }
}
