using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using System.Text.RegularExpressions;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class CreditCard : Form
    {
        Dictionary<string, string> glMapping = new Dictionary<string, string>()
        {
            { "640025", "Equipment IT" },
            { "605065", "IT Expense" }
        };

        string folderPath = @"\\able-fs03\IT Software\Credit Card\Reports";

        public CreditCard()
        {
            InitializeComponent();

            CheckAndCreateReportFile();

            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.DataError += dataGridView1_DataError; // Add DataError handler

            comboBoxFiles.SelectedIndexChanged += comboBoxFiles_SelectedIndexChanged;

            LoadAvailableFiles();
        }

        private void CheckAndCreateReportFile()
        {
            try
            {
                // Get the current date
                DateTime currentDate = DateTime.Now;

                // Determine the report period (10th of the month to the 9th of the next month)
                DateTime periodStart, periodEnd;
                string monthName;
                int year;

                if (currentDate.Day >= 10)
                {
                    periodStart = new DateTime(currentDate.Year, currentDate.Month, 10);
                    periodEnd = periodStart.AddMonths(1).AddDays(-1);
                    monthName = currentDate.ToString("MMMM"); // Current month name
                    year = currentDate.Year; // Current year
                }
                else
                {
                    periodStart = new DateTime(currentDate.Year, currentDate.Month - 1, 10);
                    periodEnd = periodStart.AddMonths(1).AddDays(-1);
                    monthName = periodStart.ToString("MMMM"); // Previous month name
                    year = periodStart.Year; // Previous month year
                }

                // Create the file name based on the period (e.g., "September 2024.csv")
                string expectedFileName = $"{monthName} {year}.csv";
                string expectedFilePath = Path.Combine(folderPath, expectedFileName);

                // Check if the file exists
                if (!File.Exists(expectedFilePath))
                {
                    // If the file doesn't exist, create an empty CSV file
                    File.Create(expectedFilePath).Dispose(); // Creates and immediately disposes the file handle

                }
                else
                {
                    // File already exists, do nothing                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking/creating report file: {ex.Message}");
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Log or show a message to handle the error
            MessageBox.Show("An error occurred: " + e.Exception.Message);
            e.ThrowException = false; // Prevent exception from being thrown
        }

        private void comboBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = comboBoxFiles.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(selectedFile))
            {
                string filePath = Path.Combine(folderPath, selectedFile + ".csv");
                ImportCSV(filePath); // Load the selected CSV file

                CheckInvoicesForReport(selectedFile);
            }
        }

        private void ImportCSV(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                dataGridView1.Rows.Clear();

                // Assuming the first line does NOT contain headers, start reading from line 0
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');

                    // Check if the "Dimension" column (replace with the actual index of the Dimension column) has a value of "102"
                    int dimensionColumnIndex = dataGridView1.Columns["Dimension"].Index;
                    if (data[dimensionColumnIndex] == "102")
                    {
                        data[dimensionColumnIndex] = "0102"; // Replace "102" with "0102"
                    }

                    dataGridView1.Rows.Add(data);

                    // Calculate totals and VAT as if the row was edited
                    CalculateRowValues(dataGridView1.Rows[i]);

                    // Ensure valid ComboBox values for "Dimension"
                    EnsureValidComboBoxValue(dataGridView1.Rows[i], "Dimension");
                }

                UpdateTotalValue(); // Recalculate the total value after import
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing CSV: {ex.Message}");
            }
        }

        // This method applies the same logic used when a row is edited
        private void CalculateRowValues(DataGridViewRow row)
        {
            // Handle VAT Code or Total change
            string vatCode = row.Cells["VAT"].Value?.ToString();
            if (decimal.TryParse(row.Cells["Total"].Value?.ToString(), out decimal total))
            {
                decimal updatedValue = 0;

                if (vatCode == "I1")
                {
                    updatedValue = total * 1.2m; // Increase by 20%
                }
                else if (vatCode == "I2")
                {
                    updatedValue = total; // No increase
                }

                row.Cells["Value"].Value = updatedValue.ToString("0.00");
            }
        }

        private void LoadAvailableFiles()
        {
            try
            {
                // Get all CSV files from the folder
                var files = Directory.GetFiles(folderPath, "*.csv")
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .Where(f => f != null)
                                     .OrderByDescending(f => DateTime.ParseExact(f, "MMMM yyyy", null))
                                     .ToList();

                comboBoxFiles.DataSource = files;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}");
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

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

                // Logic for auto-populating fields based on the base description
                if (baseDescription.Equals("Amazon", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I1";           // VAT 20%
                }
                else if (baseDescription.Equals("CJS Keys", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I2";           // VAT 0%
                }
                else if (baseDescription.Equals("Meeting Room App", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I2";           // VAT 0%
                }
                else if (baseDescription.Equals("Dell", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I1";           // VAT 0%
                }
                else if (baseDescription.Equals("Microsoft", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I1";           // VAT 0%
                }

                else if (baseDescription.Equals("Starlink", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I2";           // VAT 0%
                }

                else if (baseDescription.Equals("Ebuyer", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I1";           // VAT 0%
                }

                else if (baseDescription.Equals("Barcode Warehouse", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102
                    row.Cells["VAT"].Value = "I1";           // VAT 0%
                }

                else if (baseDescription.Equals("CCL", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102                    
                }

                else if (baseDescription.Equals("Broadband Buyer", StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["GLAccount"].Value = "605065"; // IT Expense
                    row.Cells["Dimension"].Value = "0102";    // Set Dimension to 0102                    
                }

                // Handle GL Account auto-population (based on GLAccount logic)
                string glAccountValue = row.Cells["GLAccount"].Value?.ToString();

                if (!string.IsNullOrEmpty(glAccountValue) && glMapping.ContainsKey(glAccountValue))
                {
                    row.Cells["GLName"].Value = glMapping[glAccountValue];

                    // Automatically set Dimension to 0102 if GL Account is "640025" or "605065"
                    if (glAccountValue == "640025" || glAccountValue == "605065")
                    {
                        row.Cells["Dimension"].Value = "0102"; // Set Dimension to 0102
                    }
                }
                else
                {
                    row.Cells["GLName"].Value = ""; // Clear GLName if GLAccount is not valid
                }

                // Recalculate total if VAT or total changes
                string vatCode = row.Cells["VAT"].Value?.ToString();
                if (decimal.TryParse(row.Cells["Total"].Value?.ToString(), out decimal total))
                {
                    decimal updatedValue = 0;

                    if (vatCode == "I1")
                    {
                        updatedValue = total * 1.2m; // Increase by 20%
                    }
                    else if (vatCode == "I2")
                    {
                        updatedValue = total; // No increase
                    }

                    row.Cells["Value"].Value = updatedValue.ToString("0.00");
                }
            }

            // Update the total value after changes
            UpdateTotalValue();
        }

        // Helper function to extract the base description by removing trailing numbers and spaces
        private string GetBaseDescription(string description)
        {
            // Use regex to remove trailing numbers and spaces from the description
            return Regex.Replace(description, @"\s+\d+$", "").Trim();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Call ApplyLogicForRow when a cell value changes
            ApplyLogicForRow(e.RowIndex);
        }

        private void UpdateTotalValue()
        {
            decimal totalValueSum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Value"].Value != null && decimal.TryParse(row.Cells["Value"].Value.ToString(), out decimal value))
                {
                    totalValueSum += value;
                }
            }

            TotalValue.Text = totalValueSum.ToString("0.00");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            {
                // Get the currently selected file
                string selectedFile = comboBoxFiles.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedFile))
                {
                    string filePath = Path.Combine(folderPath, selectedFile + ".csv");

                    SaveCSV(filePath); // Save changes back to the selected CSV file
                }
                else
                {
                    MessageBox.Show("No file selected to save.");
                }
            }
        }

        private void SaveCSV(string filePath)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        // Check for incomplete data (optional based on your requirements)
                        if (row.Cells["Description"].Value == null || row.Cells["Total"].Value == null || row.Cells["GLAccount"].Value == null)
                        {
                            // Skip the row if essential data is missing
                            continue;
                        }

                        // Add row data
                        string[] cells = row.Cells.Cast<DataGridViewCell>()
                                                     .Select(cell => cell.Value?.ToString() ?? "")
                                                     .ToArray();
                        csvContent.AppendLine(string.Join(",", cells));
                    }
                }

                // Write the updated content back to the CSV file
                File.WriteAllText(filePath, csvContent.ToString());                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving CSV: {ex.Message}");
            }
        }

        private void EnsureValidComboBoxValue(DataGridViewRow row, string columnName)
        {
            var comboBoxColumnIndex = dataGridView1.Columns[columnName].Index;
            var comboBoxCell = row.Cells[comboBoxColumnIndex] as DataGridViewComboBoxCell;

            if (comboBoxCell != null && comboBoxCell.Value != null)
            {
                // Ensure the current value exists in the ComboBox items list
                if (!comboBoxCell.Items.Contains(comboBoxCell.Value))
                {
                    // Set a default value if it's invalid
                    comboBoxCell.Value = comboBoxCell.Items.Count > 0 ? comboBoxCell.Items[0] : null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row.");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            string description = selectedRow.Cells["Description"].Value?.ToString();
            string total = selectedRow.Cells["Total"].Value?.ToString();
            string reportMonth = comboBoxFiles.SelectedItem.ToString(); // Get the selected report month from ComboBox

            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(total) || string.IsNullOrEmpty(reportMonth))
            {
                MessageBox.Show("Description, Total, or Report Month is missing.");
                return;
            }

            // Select the PDF file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                openFileDialog.Title = "Select PDF file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPdfFilePath = openFileDialog.FileName;

                    // Create the destination folder path and the new file name
                    string destinationFolderPath = @"\\able-fs03\IT Software\Credit Card\Invoices";
                    string newFileName = $"{description} {total} {reportMonth}.pdf";
                    string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);

                    try
                    {
                        // Copy the PDF to the new location with the new name
                        File.Copy(selectedPdfFilePath, destinationFilePath, true);

                        // Update the DataGridView to mark the invoice as imported
                        selectedRow.Cells["InvoiceImported"].Value = "Yes";
                        MessageBox.Show($"Invoice '{newFileName}' imported successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error importing the invoice: {ex.Message}");
                    }
                }
            }

            // Get the currently selected file
            string selectedFile = comboBoxFiles.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedFile))
            {
                string filePath = Path.Combine(folderPath, selectedFile + ".csv");

                SaveCSV(filePath); // Save changes back to the selected CSV file
            }
            else
            {
                MessageBox.Show("No file selected to save.");
            }
        }

        private void CheckInvoicesForReport(string reportMonth)
        {
            string invoiceFolderPath = @"\\able-fs03\IT Software\Credit Card\Invoices";

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string description = row.Cells["Description"].Value?.ToString();
                    string total = row.Cells["Total"].Value?.ToString();

                    if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(total))
                    {
                        // Construct the expected file name
                        string expectedFileName = $"{description} {total} {reportMonth}.pdf";
                        string expectedFilePath = Path.Combine(invoiceFolderPath, expectedFileName);

                        // Check if the file exists and update the "InvoiceImported" column
                        if (File.Exists(expectedFilePath))
                        {
                            row.Cells["InvoiceImported"].Value = "Yes";
                        }
                        else
                        {
                            row.Cells["InvoiceImported"].Value = "No";
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected month and year from the ComboBox
                string selectedMonthYear = comboBoxFiles.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedMonthYear))
                {
                    MessageBox.Show("Please select a month and year.");
                    return;
                }

                // Set the folder where individual PDFs are stored (assumed location)
                string pdfFolderPath = @"\\able-fs03\IT Software\Credit Card\Invoices";

                // Fetch all PDFs for the selected month and year
                var pdfFiles = Directory.GetFiles(pdfFolderPath, $"*{selectedMonthYear}*.pdf");

                if (pdfFiles.Length == 0)
                {
                    MessageBox.Show($"No PDFs found for {selectedMonthYear}.");
                    return;
                }

                // Set the destination path for the combined PDF
                string combinedPdfFolderPath = @"\\able-fs03\IT Software\Credit Card\Combined Invoices";
                string combinedPdfFileName = $"Combined {selectedMonthYear}.pdf";
                string combinedPdfFilePath = Path.Combine(combinedPdfFolderPath, combinedPdfFileName);

                // Ensure the combined folder path exists
                if (!Directory.Exists(combinedPdfFolderPath))
                {
                    Directory.CreateDirectory(combinedPdfFolderPath);
                }

                // Create the combined PDF document
                using (PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument())
                {
                    foreach (var file in pdfFiles)
                    {
                        // Open each PDF file
                        PdfSharp.Pdf.PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                        // Iterate through each page of the source document and add it to the output document
                        for (int i = 0; i < inputDocument.PageCount; i++)
                        {
                            PdfPage page = inputDocument.Pages[i];
                            outputDocument.AddPage(page);
                        }
                    }

                    // Save the combined PDF to the specified file path
                    outputDocument.Save(combinedPdfFilePath);
                }

                MessageBox.Show($"Combined PDF created successfully: {combinedPdfFileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error merging PDFs: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                openFileDialog.Title = "Select PDF file(s)";
                openFileDialog.Multiselect = true; // Allow multiple file selection

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Process each selected PDF file
                    foreach (var selectedPdfFilePath in openFileDialog.FileNames)
                    {
                        ExtractDataFromPdfAndPopulate(selectedPdfFilePath);
                    }
                }
            }
        }

        private void ExtractDataFromPdfAndPopulate(string filePath)
        {
            try
            {
                string extractedText = ExtractTextFromPdf(filePath);

                // Detect supplier
                string supplier = DetectSupplier(extractedText);
                if (supplier == "Unknown Supplier")
                {
                    MessageBox.Show($"Could not identify a known supplier from the invoice: {Path.GetFileName(filePath)}. Skipping this file.");
                    return;
                }

                // Extract total
                string total = ExtractTotalFromPdf(extractedText, supplier);
                if (string.IsNullOrEmpty(total))
                {
                    MessageBox.Show($"Unable to find the total value for supplier '{supplier}' in the PDF: {Path.GetFileName(filePath)}. Skipping this file.");
                    return;
                }

                // Ensure the description is unique
                string description = GetUniqueDescription(supplier);

                // Add the row to the DataGridView
                int rowIndex = dataGridView1.Rows.Add(description, "", "", "", "", total, total);

                // Apply logic to the new row
                ApplyLogicForRow(rowIndex);

                // Attempt to copy the PDF and update the CSV
                CopyInvoicePdf(description, total, comboBoxFiles.SelectedItem.ToString(), filePath, rowIndex);

                SaveCurrentCsvFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading or processing PDF '{Path.GetFileName(filePath)}': {ex.Message}. Skipping this file.");
            }
        }

        private void SaveCurrentCsvFile()
        {
            string selectedFile = comboBoxFiles.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedFile))
            {
                string filePath = Path.Combine(folderPath, selectedFile + ".csv");
                SaveCSV(filePath); // Save changes back to the selected CSV file
            }
            else
            {
                MessageBox.Show("No file selected to save.");
            }
        }

        private void CopyInvoicePdf(string description, string total, string reportMonth, string sourceFilePath, int rowIndex)
        {
            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(total) || string.IsNullOrEmpty(reportMonth))
            {
                MessageBox.Show("Description, Total, or Report Month is missing.");
                return;
            }

            // Create the destination folder path and the new file name
            string destinationFolderPath = @"\\able-fs03\IT Software\Credit Card\Invoices";
            string newFileName = $"{description} {total} {reportMonth}.pdf";
            string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);

            try
            {
                // Copy the PDF to the new location with the new name
                File.Copy(sourceFilePath, destinationFilePath, true);

                // Update the DataGridView to mark the invoice as imported
                dataGridView1.Rows[rowIndex].Cells["InvoiceImported"].Value = "Yes";
                //MessageBox.Show($"Invoice '{newFileName}' imported successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing the invoice: {ex.Message}");
            }
        }

        private string GetUniqueDescription(string description)
        {
            int count = 1;
            string originalDescription = description;

            // Check if the description already exists in the DataGridView
            while (DoesDescriptionExist(description))
            {
                description = $"{originalDescription} {count}";
                count++;
            }

            return description;
        }

        private bool DoesDescriptionExist(string description)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["Description"].Value != null)
                {
                    if (row.Cells["Description"].Value.ToString().Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string ExtractTextFromPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();

            using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    // Use PdfPig's content extraction method for better accuracy
                    text.Append(ContentOrderTextExtractor.GetText(page));
                }
            }

            return text.ToString();
        }

        private string DetectSupplier(string extractedText)
        {
            if (extractedText.Contains("Microsoft 365 Business Basic"))
            {
                return "Microsoft";
            }
            else if (extractedText.Contains("CJS CD Keys"))
            {
                return "CJS Keys";
            }
            else if (extractedText.Contains("STARLINK", StringComparison.OrdinalIgnoreCase))
            {
                return "Starlink";
            }
            else if (extractedText.Contains("Ebuyer", StringComparison.OrdinalIgnoreCase))
            {
                return "Ebuyer";
            }
            else if (extractedText.Contains("The Barcode Warehouse", StringComparison.OrdinalIgnoreCase))
            {
                return "Barcode Warehouse";
            }
            else if (extractedText.Contains("Amazon", StringComparison.OrdinalIgnoreCase))
            {
                return "Amazon";
            }
            else if (extractedText.Contains("ccl", StringComparison.OrdinalIgnoreCase))
            {
                return "CCL";
            }
            else if (extractedText.Contains("broadbandbuyer", StringComparison.OrdinalIgnoreCase))
            {
                return "Broadband Buyer";
            }
            else
            {
                return "Unknown Supplier";
            }
        }

        private string ExtractTotalFromPdf(string extractedText, string supplier)
        {
            switch (supplier)
            {
                case "Microsoft":
                    // Extract the charges for Microsoft
                    var matchMicrosoft = Regex.Match(extractedText, @"Charges:\s*([0-9]*\.?[0-9]+)");
                    if (matchMicrosoft.Success)
                    {
                        return matchMicrosoft.Groups[1].Value; // Extracted charges (e.g., "4.90")
                    }
                    break;

                case "CJS Keys":
                    // Extract the total cost for CJS Keys (removing £ symbol)
                    var matchCjsKeys = Regex.Match(extractedText, @"Total Cost:\s*£([0-9]*\.?[0-9]+)");
                    if (matchCjsKeys.Success)
                    {
                        string totalCost = matchCjsKeys.Groups[1].Value; // Extracted total cost (e.g., "9.99")
                        return totalCost; // Return the value without the £ symbol
                    }
                    break;

                case "Starlink":
                    // Extract the total charges for Starlink (removing £ symbol)
                    var matchStarlink = Regex.Match(extractedText, @"Total Charges\s*£([0-9]*\.?[0-9]+)");
                    if (matchStarlink.Success)
                    {
                        string totalCharges = matchStarlink.Groups[1].Value; // Extracted total charges (e.g., "96.00")
                        return totalCharges; // Return the value without the £ symbol
                    }
                    break;

                case "Ebuyer":
                    // Extract the total charges for Starlink (removing £ symbol)
                    var matchEbuyer = Regex.Match(extractedText, @"Subtotal\s*([0-9]*\.?[0-9]+)");
                    if (matchEbuyer.Success)
                    {
                        string totalCharges = matchEbuyer.Groups[1].Value; // Extracted total charges (e.g., "96.00")
                        return totalCharges; // Return the value without the £ symbol
                    }
                    break;

                case "Barcode Warehouse":
                    // Refined regex pattern to capture "Total (ex VAT) £ 156.85" with flexible spacing
                    var matchBarcode = Regex.Match(extractedText, @"Total \(ex VAT\)\s*£\s*([0-9]*\.?[0-9]+)");
                    if (matchBarcode.Success)
                    {
                        return matchBarcode.Groups[1].Value; // Extracted total charges (e.g., "156.85")
                    }
                    break;

                case "Amazon":
                    // Updated regex pattern to match something like "Total £1,000.00" or "Total £22.46"
                    var matchAmazon = Regex.Match(extractedText, @"Total\s*£\s*([0-9,]*\.?[0-9]+)");
                    if (matchAmazon.Success)
                    {
                        // Extract the matched total value
                        string rawTotal = matchAmazon.Groups[1].Value;

                        // Remove commas to handle thousands separators
                        rawTotal = rawTotal.Replace(",", "");

                        return rawTotal; // Now returns something like "1000.00" instead of "1"
                    }
                    break;

                case "CCL":
                    // Regex to match "SUBTOTAL 69.99" format for CCL
                    var matchCclSubtotal = Regex.Match(extractedText, @"SUBTOTAL\s*([0-9]*\.?[0-9]+)");
                    if (matchCclSubtotal.Success)
                    {
                        return matchCclSubtotal.Groups[1].Value; // Extracted subtotal (e.g., "69.99")
                    }

                    // Regex to match "TOTAL inc. VAT £ 92.98" format for CCL
                    var matchCclTotalIncVat = Regex.Match(extractedText, @"TOTAL inc\. VAT\s*£\s*([0-9]*\.?[0-9]+)");
                    if (matchCclTotalIncVat.Success)
                    {
                        return matchCclTotalIncVat.Groups[1].Value; // Extracted total including VAT (e.g., "92.98")
                    }
                    break;

                case "Broadband Buyer":
                    // Updated regex pattern to match "Total £22.46" format
                    var matchBBB = Regex.Match(extractedText, @"Sub Total\s*£\s*([0-9]*\.?[0-9]+)");
                    if (matchBBB.Success)
                    {
                        return matchBBB.Groups[1].Value; // Extracted pre-VAT total (e.g., "22.46")
                    }
                    break;


                // Add more cases for other suppliers as needed

                default:
                    return null;
            }

            return null; // Return null if no matching total was found
        }
    }
}
