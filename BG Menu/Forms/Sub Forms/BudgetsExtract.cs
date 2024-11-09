using Google.Cloud.Storage.V1;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class BudgetsExtract : Form
    {

        private SQLiteConnection sqlConnection;
        private FirebaseStorage firebaseStorage;

        public BudgetsExtract()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Required by EPPlus

            var storageClient = StorageClient.Create(); // Assumes authentication via default Google credentials
            firebaseStorage = new FirebaseStorage(storageClient);

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Location", "Location");
            dataGridView1.Columns.Add("Week", "Week");
            dataGridView1.Columns.Add("Value", "Value");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsm;*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    await ImportExcelDataAsync(filePath);
                }
            }
        }

        private Task ImportExcelDataAsync(string filePath)
        {
            return Task.Run(() =>
            {
                var excludedRows = new HashSet<int> { 13, 14, 19, 20, 25, 26, 32, 33, 38, 39, 44, 45, 51, 52, 57, 58, 63, 64, 70, 71, 76, 77 }; // Dont Count These Cells, They Are Blank Or Calc Cells

                var dictionaries = new Dictionary<string, string>
                {
                    // UK Stores
                    {"Birkenhead", "Birkenhead"},
                    {"Burton", "Burton"},
                    {"Chester", "Chester"},
                    {"Congleton", "Congleton"},
                    {"Crewe", "Crewe"},
                    {"Darlington", "Darlington"},
                    {"Gloucester", "Gloucester"},
                    {"Hanley", "Hanley"},
                    {"Lincoln", "Lincoln"},
                    {"Llandudno", "Llandudno"},
                    {"Nantwich", "Nantwich"},
                    {"Newark", "Newark"},
                    {"Newport", "Newport"},
                    {"Northwich", "Northwich"},
                    {"Oswestry", "Oswestry"},
                    {"Queensferry", "Queensferry"},
                    {"Reading", "Reading"},
                    {"Rhyl", "Rhyl"},
                    {"Runcorn", "Runcorn"},
                    {"Shrewsbury", "Shrewsbury"},
                    {"Specialist", "Specialist"},
                    {"Stafford", "Stafford"},
                    {"Stairlift", "Stairlifts"},
                    {"Stockport", "Stockport"},
                    {"Stockton", "Stockton"},
                    {"Thatcham", "Thatcham"},
                    {"Wrexham", "Wrexham"},
                    {"Engineering", "Engineering"},
                    {"HO", "Dropship"},

                    // Franchsie Stores
                    {"Blackpool", "Blackpool"},
                    {"Bridgend", "Bridgend"},
                    {"Broxburn", "Broxburn"},
                    {"Cardiff", "Cardiff"},
                    {"Christchurch", "Christchurch"},
                    {"Colchester", "Colchester"},
                    {"Hyde", "Hyde"},
                    {"Leeds", "Leeds"},
                    {"Newport_SW", "Newport Wales"},
                    {"Paisley", "Paisley"},
                    {"Salford", "Salford"},
                    {"Southampton", "Southampton"},
                    {"Southport", "Southport"},
                    {"St_Helens", "St Helens"},
                    {"Wavertree", "Wavertree"},
                    {"Wigan", "Wigan"},

                    // Franchsie Companies
                    {"Broxburn_Total", "AMD"},
                    {"Leeds_Total", "AWG"},
                    {"Colchester_Total", "GRMR"},
                    {"JSCD_Total", "JSCD"},
                    {"Mob_GB_Total", "Mobility GB"},
                    {"Paisley_Total", "SJLK"},
                    {"SML_Total", "SML"}
                };

                // Initialize a list to hold the data temporarily
                var dataToDisplay = new List<Tuple<string, int, double>>();

                string cellRange = textBox1.Text;

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var entry in dictionaries)
                    {
                        var sheetName = entry.Key;
                        var locationName = entry.Value;

                        var worksheet = package.Workbook.Worksheets[sheetName];
                        if (worksheet == null)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show($"Worksheet {sheetName} not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                            continue;
                        }

                        // Fetch the entire data range at once for better performance
                        var data = worksheet.Cells[$"{cellRange}"].ToList();

                        int weekNumber = 1;
                        for (int i = 0; i < data.Count; i++)
                        {
                            int rowIndex = data[i].Start.Row;
                            if (excludedRows.Contains(rowIndex)) continue;

                            // Remove currency symbols and commas
                            string cellValue = data[i].Text.Trim().Replace("£", "").Replace(",", "");

                            if (string.IsNullOrWhiteSpace(cellValue))
                            {
                                cellValue = "0"; // Treat empty or null cells as 0
                            }

                            if (double.TryParse(cellValue, out double value))
                            {
                                dataToDisplay.Add(new Tuple<string, int, double>(locationName, weekNumber, value));
                            }
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    MessageBox.Show($"Non-numeric value '{cellValue}' found in {sheetName} at row {rowIndex}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }));
                                continue;
                            }

                            weekNumber++;
                        }
                    }
                }

                // Batch update the DataGridView on the UI thread
                if (dataToDisplay.Any())
                {
                    Invoke(new Action(() =>
                    {
                        dataGridView1.SuspendLayout();
                        foreach (var item in dataToDisplay)
                        {
                            dataGridView1.Rows.Add(item.Item1, item.Item2, item.Item3);
                        }
                        dataGridView1.ResumeLayout();
                    }));
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("No data loaded into the DataGridView. Please check the Excel file for valid data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dbFolderPath = Path.Combine(appDataPath, "BG-Menu");
            string dbFilePath = Path.Combine(dbFolderPath, "Targets.db"); // Replace with your actual database file name

            if (!File.Exists(dbFilePath))
            {
                MessageBox.Show("Database file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadDatabase(dbFilePath);            
        }

        private void LoadDatabase(string dbFilePath)
        {
            try
            {
                if (!File.Exists(dbFilePath))
                {
                    MessageBox.Show("Database file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }

                // Ensure you are opening the database with write permissions
                sqlConnection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;Read Only=False;");
                sqlConnection.Open();

                // Populate ComboBox with table names
                using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", sqlConnection))
                using (var reader = cmd.ExecuteReader())
                {
                    comboBoxTables.Items.Clear();
                    while (reader.Read())
                    {
                        comboBoxTables.Items.Add(reader["name"].ToString());
                    }
                }

                if (comboBoxTables.Items.Count > 0)
                {
                    comboBoxTables.SelectedIndex = 0;
                    LoadSelectedTableData();
                }

                MessageBox.Show("Database loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Unable to open database file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedTableData();
        }

        private void LoadSelectedTableData()
        {
            if (sqlConnection == null || comboBoxTables.SelectedItem == null)
            {
                return;
            }

            string selectedTable = comboBoxTables.SelectedItem.ToString();
            string query = $"SELECT * FROM {selectedTable}";

            using (var cmd = new SQLiteCommand(query, sqlConnection))
            using (var adapter = new SQLiteDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridViewDB.DataSource = table;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (sqlConnection == null || comboBoxTables.SelectedItem == null)
            {
                MessageBox.Show("Please load a database and select a table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedTable = comboBoxTables.SelectedItem.ToString();
            UpdateDatabase(selectedTable);
        }


        private void UpdateDatabase(string tableName)
        {
            try
            {
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string store = row.Cells["Location"].Value?.ToString();
                        int week = Convert.ToInt32(row.Cells["Week"].Value);
                        double value = Convert.ToDouble(row.Cells["Value"].Value);

                        string column = tableName.Contains("Sales") ? "Sales" : "Target";

                        // First, check if the record exists
                        string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE Store = @Store AND Week = @Week";
                        using (var checkCmd = new SQLiteCommand(checkQuery, sqlConnection, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@Store", store);
                            checkCmd.Parameters.AddWithValue("@Week", week);
                            long count = (long)checkCmd.ExecuteScalar();

                            if (count > 0)
                            {
                                // Record exists, perform an update
                                string updateQuery = $"UPDATE {tableName} SET {column} = @Value WHERE Store = @Store AND Week = @Week";
                                using (var updateCmd = new SQLiteCommand(updateQuery, sqlConnection, transaction))
                                {
                                    updateCmd.Parameters.AddWithValue("@Store", store);
                                    updateCmd.Parameters.AddWithValue("@Week", week);
                                    updateCmd.Parameters.AddWithValue("@Value", value);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Record doesn't exist, perform an insert
                                string insertQuery = $"INSERT INTO {tableName} (Store, Week, {column}) VALUES (@Store, @Week, @Value)";
                                using (var insertCmd = new SQLiteCommand(insertQuery, sqlConnection, transaction))
                                {
                                    insertCmd.Parameters.AddWithValue("@Store", store);
                                    insertCmd.Parameters.AddWithValue("@Week", week);
                                    insertCmd.Parameters.AddWithValue("@Value", value);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    transaction.Commit();
                }

                MessageBox.Show("Database updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSelectedTableData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Call the upload method from FirebaseStorage class
                bool success = await firebaseStorage.UploadBudgetsCalcFileFromBudgetsReportFolderAsync(sqlConnection);
                if (success)
                {
                    MessageBox.Show("The database file has been uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
