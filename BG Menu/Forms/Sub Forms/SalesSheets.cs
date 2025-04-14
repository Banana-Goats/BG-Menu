using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class SalesSheets : Form
    {
        private string masterFilePath;
        private BindingList<Mapping> mappings;
        private string yearSegment = "2024-25";

        // Hard-coded list of Tabs
        private Dictionary<string, string> GetSalesSheetsDictionaryFromDb()
        {
            var result = new Dictionary<string, string>();

            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
            string query = "SELECT ExcelTab, Company FROM Store_Mapping WHERE SalesSheets = 'Yes';";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string excelTab = reader["ExcelTab"].ToString();
                        string company = reader["Company"].ToString();
                        result[excelTab] = company;
                    }
                }
            }

            return result;
        }

        public SalesSheets()
        {
            InitializeComponent();
            ExcelPackage.License.SetNonCommercialOrganization("Ableworld");
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            mappings = new BindingList<Mapping>();

            dataGridViewMappings.AutoGenerateColumns = false;

            var tabColumn = new DataGridViewTextBoxColumn { HeaderText = "Tab", DataPropertyName = "Tab", Name = "Tab", Width = 150 };
            var fileColumn = new DataGridViewTextBoxColumn { HeaderText = "File", DataPropertyName = "File", Name = "File", Width = 300 };
            var foundColumn = new DataGridViewTextBoxColumn { HeaderText = "Found", DataPropertyName = "Found", Name = "Found", Width = 100 };
            var copyStatusColumn = new DataGridViewTextBoxColumn { HeaderText = "Copy Status", DataPropertyName = "CopyStatus", Name = "CopyStatus", Width = 150 };

            dataGridViewMappings.Columns.AddRange(tabColumn, fileColumn, foundColumn, copyStatusColumn);

            dataGridViewMappings.DataSource = mappings;
        }

        private void InitializeMappings(string yearSegment)
        {
            mappings.Clear();

            // Fetch dictionary from the database
            var dictionaryFromDb = GetSalesSheetsDictionaryFromDb();

            foreach (var entry in dictionaryFromDb)
            {
                string tab = entry.Key;
                string folder = entry.Value;
                mappings.Add(new Mapping { Tab = tab, File = $"{tab} Sales {yearSegment}.xlsx", Folder = folder, Found = "No" });
            }
            dataGridViewMappings.Refresh();
        }

        private void btnLoadMaster_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsm";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    masterFilePath = openFileDialog.FileName;
                    lblStatus.Text = "Status: Master spreadsheet loaded.";
                }
            }
        }

        private async void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = folderBrowserDialog.SelectedPath;
                    lblStatus.Text = "Status: Store folder selected.";

                    string masterFilePath = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*Master Sales*.xlsx").FirstOrDefault();
                    if (string.IsNullOrEmpty(masterFilePath))
                    {
                        MessageBox.Show("The 'Master Sales' file could not be found in the selected folder.");
                        return;
                    }

                    var match = System.Text.RegularExpressions.Regex.Match(Path.GetFileName(masterFilePath), @"Master Sales (\d{4}-\d{2}).xlsx");
                    if (match.Success)
                    {
                        yearSegment = match.Groups[1].Value;
                    }
                    else
                    {
                        MessageBox.Show("Unable to extract the year from the 'Master Sales' file name.");
                        return;
                    }

                    InitializeMappings(yearSegment);
                    CheckForStoreFiles(folderBrowserDialog.SelectedPath);

                    lblStatus.Text = "Status: Mappings initialized and files checked.";
                }
            }

            await CreateFiles();
        }

        private void CheckForStoreFiles(string baseFolderPath)
        {
            Parallel.ForEach(mappings, mapping =>
            {
                // Construct the full folder path
                string folderPath = Path.Combine(baseFolderPath, mapping.Folder);
                string expectedFilePath = Path.Combine(folderPath, mapping.File);

                if (File.Exists(expectedFilePath))
                {
                    mapping.Found = "Yes";
                }
                else
                {
                    mapping.Found = "No";
                }
            });

            dataGridViewMappings.Refresh();
        }

        private void SetRowColor(string tabName, Color color)
        {
            foreach (DataGridViewRow row in dataGridViewMappings.Rows)
            {
                if (row.Cells["Tab"].Value != null && row.Cells["Tab"].Value.ToString() == tabName)
                {
                    row.Cells["Found"].Style.BackColor = color;
                    break;
                }
            }
        }

        private async void btnExecuteCopyPaste_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(masterFilePath))
            {
                MessageBox.Show("Please load the master spreadsheet first.");
                return;
            }

            string copyRange1 = txtCopyRange.Text;
            string pasteRange1 = txtPasteRange.Text;
            string copyRange2 = txtCopyRange2.Text;
            string pasteRange2 = txtPasteRange2.Text;

            if (string.IsNullOrEmpty(copyRange1) || string.IsNullOrEmpty(pasteRange1) ||
                string.IsNullOrEmpty(copyRange2) || string.IsNullOrEmpty(pasteRange2))
            {
                MessageBox.Show("Please specify both copy and paste ranges.");
                return;
            }

            lblStatus.Text = "Status: Copying data, please wait...";

            await Task.Run(() => ExecuteCopyPaste(masterFilePath, copyRange1, pasteRange1, copyRange2, pasteRange2));

            lblStatus.Text = "Status: Data copy-paste operation completed.";
            MessageBox.Show("Data copy-paste operation completed.");
        }

        private void ExecuteCopyPaste(string masterFilePath, string copyRange1, string pasteRange1, string copyRange2, string pasteRange2)
        {
            using (var package = new ExcelPackage(new FileInfo(masterFilePath)))
            {
                var workbook = package.Workbook;

                Parallel.ForEach(mappings.Where(m => m.Found == "Yes"), mapping =>
                {
                    try
                    {
                        var masterSheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == mapping.Tab);
                        if (masterSheet == null)
                        {
                            this.Invoke(new Action(() =>
                            {
                                mapping.CopyStatus = "Skipped (Tab not found)";
                                SetRowColor(mapping.Tab, Color.Orange);
                            }));
                            return;
                        }

                        // Construct the folder path for the store file
                        string folderPath = Path.Combine(txtFolderPath.Text, mapping.Folder);
                        string storeFilePath = Path.Combine(folderPath, mapping.File);

                        using (var storePackage = new ExcelPackage(new FileInfo(storeFilePath)))
                        {
                            var storeWorkbook = storePackage.Workbook;
                            var storeSheet = storeWorkbook.Worksheets[0];

                            storeSheet.Cells[pasteRange1].Value = masterSheet.Cells[copyRange1].Value;
                            storeSheet.Cells[pasteRange2].Value = masterSheet.Cells[copyRange2].Value;

                            storeSheet.Calculate();
                            storePackage.Save();
                        }

                        this.Invoke(new Action(() =>
                        {
                            mapping.CopyStatus = "Copy-Paste Success";
                            SetRowColor(mapping.Tab, Color.Green);
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            mapping.CopyStatus = $"Copy-Paste Failed: {ex.Message}";
                            SetRowColor(mapping.Tab, Color.Red);
                        }));
                    }
                });
            }

            this.Invoke(new Action(() => dataGridViewMappings.Refresh()));
        }

        public class Mapping
        {
            public string Tab { get; set; }
            public string File { get; set; }
            public string Folder { get; set; } 
            public string Found { get; set; }
            public string CopyStatus { get; set; }
        }

        private async Task CreateFiles()
        {
            string folderPath = txtFolderPath.Text;
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please select the store folder first.");
                return;
            }

            string masterFilePath = Directory.GetFiles(folderPath, "*Master Sales*.xlsx").FirstOrDefault();
            if (string.IsNullOrEmpty(masterFilePath))
            {
                MessageBox.Show("The 'Master Sales' file could not be found in the selected folder.");
                return;
            }

            var match = System.Text.RegularExpressions.Regex.Match(Path.GetFileName(masterFilePath), @"Master Sales (\d{4}-\d{2}).xlsx");
            if (!match.Success)
            {
                MessageBox.Show("Unable to extract the year from the 'Master Sales' file name.");
                return;
            }

            string yearSegment = match.Groups[1].Value;

            lblStatus.Text = "Status: Creating store files, please wait...";

            await Task.Run(() => CreateStoreFiles(masterFilePath, folderPath, yearSegment));

            lblStatus.Text = "Status: Store files created, updated, and formulas refreshed successfully.";
            MessageBox.Show("Store files created, updated, and formulas refreshed successfully.");
        }

        private void dataGridViewMappings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the "Tab" column
            if (e.ColumnIndex == dataGridViewMappings.Columns["Tab"].Index)
            {
                var tabCell = dataGridViewMappings.Rows[e.RowIndex].Cells["Tab"];
                var fileCell = dataGridViewMappings.Rows[e.RowIndex].Cells["File"];

                if (tabCell.Value != null && !string.IsNullOrWhiteSpace(tabCell.Value.ToString()))
                {
                    // Automatically generate the file name based on the Tab value and the dynamic yearSegment
                    fileCell.Value = $"{tabCell.Value.ToString()} Sales {yearSegment}.xlsx";

                    // Update the mappings list
                    var mapping = mappings[e.RowIndex];
                    mapping.File = fileCell.Value.ToString();
                }
            }
        }

        private void CreateStoreFiles(string masterFilePath, string baseFolderPath, string yearSegment)
        {
            using (var package = new ExcelPackage(new FileInfo(masterFilePath)))
            {
                var workbook = package.Workbook;

                Parallel.ForEach(mappings, mapping =>
                {
                    // Ensure the folder exists
                    string folderPath = Path.Combine(baseFolderPath, mapping.Folder);
                    Directory.CreateDirectory(folderPath); // Create the directory if it doesn't exist

                    string newFileName = Path.Combine(folderPath, $"{mapping.Tab} Sales {yearSegment}.xlsx");

                    try
                    {
                        // Copy the master file to the new file
                        File.Copy(masterFilePath, newFileName, true);

                        using (var storePackage = new ExcelPackage(new FileInfo(newFileName)))
                        {
                            var storeWorkbook = storePackage.Workbook;
                            var worksheet = storeWorkbook.Worksheets[0];

                            worksheet.Cells["M1"].Value = mapping.Tab;

                            worksheet.Calculate();
                            storePackage.Save();
                        }

                        this.Invoke(new Action(() =>
                        {
                            mapping.Found = "Yes";
                            mapping.CopyStatus = $"File Created ({yearSegment})";
                            SetRowColor(mapping.Tab, Color.Green);
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            mapping.CopyStatus = $"Failed: {ex.Message}";
                            SetRowColor(mapping.Tab, Color.Red);
                        }));
                    }
                });
            }

            this.Invoke(new Action(() => dataGridViewMappings.Refresh()));
        }
    }
}
