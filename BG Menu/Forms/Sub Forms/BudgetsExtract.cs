using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class BudgetsExtract : Form
    {
        private const string ConfigFilePath = "Config.xml";

        public BudgetsExtract()
        {
            InitializeComponent();

            ExcelPackage.License.SetNonCommercialOrganization("Ableworld"); // Required by EPPlus                     

            PopulateMappingGridFromSqlTable();

            LoadMappingConfig();
        }

        private class ExportData
        {
            public string Store { get; set; }
            public int Week { get; set; }
            public double Target { get; set; }
            public double Sales2020 { get; set; }
            public double Sales2021 { get; set; }
            public double Sales2022 { get; set; }
            public double Sales2023 { get; set; }
        }

        private void PopulateMappingGridFromSqlTable()
        {
            // Get SQL Server connection string from config.
            string sqlConnStr = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
            var columns = new List<string>();

            // Query the column names from Sales.SalesData in the Sales schema.
            using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
            {
                sqlConn.Open();
                string query = @"
                    SELECT COLUMN_NAME 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'Sales' AND TABLE_NAME = 'SalesData'";
                using (SqlCommand cmd = new SqlCommand(query, sqlConn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string colName = reader["COLUMN_NAME"].ToString();
                            // Exclude the key columns (Store and Week) from the mapping.
                            if (!colName.Equals("Store", StringComparison.OrdinalIgnoreCase) &&
                                !colName.Equals("Week", StringComparison.OrdinalIgnoreCase))
                            {
                                columns.Add(colName);
                            }
                        }
                    }
                }
            }

            // Setup the mapping DataGridView with three columns: Field, Start Cell, and End Cell.
            dataGridViewMapping.Columns.Clear();
            dataGridViewMapping.Columns.Add("Field", "Field");
            dataGridViewMapping.Columns.Add("StartCell", "Start Cell");
            dataGridViewMapping.Columns.Add("EndCell", "End Cell");

            dataGridViewMapping.Columns["Field"].ReadOnly = true;

            // Populate one row per field dynamically.
            foreach (var field in columns)
            {
                // Initially the cell ranges will be blank (or you can pre-populate with defaults).
                dataGridViewMapping.Rows.Add(field, "", "");
            }
        }

        private void UpdateProgress(int percent, string message)
        {
            Invoke(new Action(() =>
            {
                progressBar1.Value = percent;
                labelProgress.Text = message;
            }));
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsm;*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    await ImportExcelDataWithMappingAsync(filePath);
                    button6.Enabled = false;
                }
            }
        }

        private Task ImportExcelDataWithMappingAsync(string filePath)
        {
            return Task.Run(() =>
            {
                UpdateProgress(0, "Starting import...");

                // 1. Read the per-field mapping from the DataGridView.
                var fieldMappings = new Dictionary<string, Tuple<string, string>>();
                foreach (DataGridViewRow row in dataGridViewMapping.Rows)
                {
                    if (row.IsNewRow) continue;

                    string fieldName = row.Cells["Field"].Value?.ToString();
                    string startCell = row.Cells["StartCell"].Value?.ToString();
                    string endCell = row.Cells["EndCell"].Value?.ToString();

                    if (!string.IsNullOrEmpty(fieldName) &&
                        !string.IsNullOrEmpty(startCell) &&
                        !string.IsNullOrEmpty(endCell))
                    {
                        fieldMappings[fieldName] = Tuple.Create(startCell, endCell);
                    }
                }

                if (fieldMappings.Count == 0)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Please provide valid cell ranges in the mapping grid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    return;
                }

                UpdateProgress(10, "Mapping grid loaded.");

                // 2. Define the rows to exclude.
                var excludedRows = new HashSet<int> { 13, 14, 19, 20, 25, 26, 32, 33, 38, 39, 44, 45, 51, 52, 57, 58, 63, 64, 70, 71, 76, 77 };

                // 3. Get the Excel tab-to-Store mapping from SQL.
                var excelTabMappings = new Dictionary<string, string>();
                string sqlConnStr = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
                using (var connection = new SqlConnection(sqlConnStr))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT ExcelTab, StoreName FROM Store_Mapping WHERE Budgets = 'Yes'", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string excelTab = reader["ExcelTab"]?.ToString();
                            string storeName = reader["StoreName"]?.ToString();
                            if (!string.IsNullOrEmpty(excelTab) && !string.IsNullOrEmpty(storeName))
                            {
                                excelTabMappings[excelTab] = storeName;
                            }
                        }
                    }
                }

                UpdateProgress(15, "Excel tab mapping retrieved.");

                // 4. Prepare a dictionary to hold the merged data.
                //    Key: Tuple of (Store, Week)
                //    Value: A dynamic dictionary mapping each field (from the mapping grid) to its value.
                var combinedData = new Dictionary<Tuple<string, int>, Dictionary<string, double>>();

                // 5. Open the Excel file.
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    int totalSheets = excelTabMappings.Count;
                    int sheetCounter = 0;
                    // Process each worksheet based on the mapping.
                    foreach (var entry in excelTabMappings)
                    {
                        sheetCounter++;
                        string sheetName = entry.Key;
                        string locationName = entry.Value;
                        UpdateProgress(15 + (sheetCounter * 30 / totalSheets), $"Processing worksheet '{sheetName}' ({sheetCounter}/{totalSheets})");

                        var worksheet = package.Workbook.Worksheets[sheetName];
                        if (worksheet == null)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show($"Worksheet {sheetName} not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                            continue;
                        }

                        // For each field mapping, extract the values from the specified cell range.
                        var fieldValues = new Dictionary<string, List<double>>();
                        int? expectedCount = null;
                        foreach (var mapping in fieldMappings)
                        {
                            string field = mapping.Key;
                            string startCell = mapping.Value.Item1;
                            string endCell = mapping.Value.Item2;
                            string rangeStr = $"{startCell}:{endCell}";

                            var cells = worksheet.Cells[rangeStr].ToList();
                            var values = new List<double>();
                            foreach (var cell in cells)
                            {
                                // Only process if this cell’s row is not excluded.
                                if (excludedRows.Contains(cell.Start.Row))
                                    continue;

                                string cellText = cell.Text.Trim().Replace("£", "").Replace(",", "");
                                if (string.IsNullOrWhiteSpace(cellText))
                                {
                                    cellText = "0";
                                }
                                if (double.TryParse(cellText, out double parsed))
                                {
                                    values.Add(parsed);
                                }
                                else
                                {
                                    Invoke(new Action(() =>
                                    {
                                        MessageBox.Show($"Non-numeric value '{cell.Text}' found in sheet {sheetName} at cell {cell.Address} for field {field}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }));
                                }
                            }

                            // Check that all field mappings yield the same number of data points.
                            if (expectedCount == null)
                            {
                                expectedCount = values.Count;
                            }
                            else if (expectedCount != values.Count)
                            {
                                Invoke(new Action(() =>
                                {
                                    MessageBox.Show($"The number of data points for field '{field}' in sheet '{sheetName}' does not match the expected count.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }));
                                continue;
                            }
                            fieldValues[field] = values;
                        }

                        // 6. Combine the extracted field values into records.
                        if (expectedCount != null)
                        {
                            for (int i = 0; i < expectedCount; i++)
                            {
                                var key = Tuple.Create(locationName, i + 1); // Week is assigned sequentially (i+1)
                                if (!combinedData.ContainsKey(key))
                                {
                                    combinedData[key] = new Dictionary<string, double>();
                                }
                                // For each dynamic field, assign the corresponding value.
                                foreach (var mapping in fieldMappings)
                                {
                                    string field = mapping.Key;
                                    double value = (fieldValues.ContainsKey(field) && fieldValues[field].Count > i) ? fieldValues[field][i] : 0;
                                    combinedData[key][field] = value;
                                }
                            }
                        }
                    }
                }

                UpdateProgress(50, "Finished processing worksheets. Updating records...");

                // 7. Update/Insert the merged data into the SQL Server table (Sales.SalesData).
                //    Build the SQL queries dynamically based on the dynamic fields.
                List<string> dynamicFields = fieldMappings.Keys.ToList();
                string sqlServerConnString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
                try
                {
                    using (var sqlConn = new SqlConnection(sqlServerConnString))
                    {
                        sqlConn.Open();
                        using (var transaction = sqlConn.BeginTransaction())
                        {
                            int totalRecords = combinedData.Count;
                            int recordCounter = 0;
                            foreach (var kvp in combinedData)
                            {
                                recordCounter++;
                                var key = kvp.Key;
                                string store = key.Item1;
                                int week = key.Item2;
                                var fieldData = kvp.Value;

                                int progressVal = 50 + (recordCounter * 45 / totalRecords);
                                UpdateProgress(progressVal, $"Updating record for {store}, Week {week} ({recordCounter}/{totalRecords})");

                                // First, check if the record exists.
                                string checkQuery = "SELECT COUNT(*) FROM Sales.SalesData WHERE Store = @Store AND Week = @Week";
                                using (var checkCmd = new SqlCommand(checkQuery, sqlConn, transaction))
                                {
                                    checkCmd.Parameters.AddWithValue("@Store", store);
                                    checkCmd.Parameters.AddWithValue("@Week", week);
                                    int count = (int)checkCmd.ExecuteScalar();

                                    if (count > 0)
                                    {
                                        // Build dynamic UPDATE query.
                                        string setClause = string.Join(", ", dynamicFields.Select(f => $"[{f}] = @{f}"));
                                        string updateQuery = $"UPDATE Sales.SalesData SET {setClause} WHERE Store = @Store AND Week = @Week";
                                        using (var updateCmd = new SqlCommand(updateQuery, sqlConn, transaction))
                                        {
                                            updateCmd.Parameters.AddWithValue("@Store", store);
                                            updateCmd.Parameters.AddWithValue("@Week", week);
                                            foreach (string field in dynamicFields)
                                            {
                                                double value = fieldData.ContainsKey(field) ? fieldData[field] : 0;
                                                updateCmd.Parameters.AddWithValue("@" + field, value);
                                            }
                                            updateCmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        // Build dynamic INSERT query.
                                        var allColumns = new List<string> { "Store", "Week" };
                                        allColumns.AddRange(dynamicFields);
                                        string columnNames = string.Join(", ", allColumns.Select(c => $"[{c}]"));
                                        string parameterNames = string.Join(", ", allColumns.Select(c => "@" + c));
                                        string insertQuery = $"INSERT INTO Sales.SalesData ({columnNames}) VALUES ({parameterNames})";
                                        using (var insertCmd = new SqlCommand(insertQuery, sqlConn, transaction))
                                        {
                                            insertCmd.Parameters.AddWithValue("@Store", store);
                                            insertCmd.Parameters.AddWithValue("@Week", week);
                                            foreach (string field in dynamicFields)
                                            {
                                                double value = fieldData.ContainsKey(field) ? fieldData[field] : 0;
                                                insertCmd.Parameters.AddWithValue("@" + field, value);
                                            }
                                            insertCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                    }
                    UpdateProgress(100, "Excel data imported successfully using dynamic mapping!");

                    // After a successful import, save the current mapping config.
                    SaveMappingConfig();

                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Excel data imported successfully using dynamic mapping!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button6.Enabled = true;
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Error exporting to SQL Server: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button6.Enabled = true;
                    }));
                }
            });
        }

        private void LoadMappingConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                // No config exists yet—first run.
                return;
            }

            try
            {
                XDocument doc = XDocument.Load(ConfigFilePath);

                foreach (DataGridViewRow row in dataGridViewMapping.Rows)
                {
                    if (row.IsNewRow) continue;
                    string fieldName = row.Cells["Field"].Value?.ToString();
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        var fieldElement = doc.Root.Elements("Field")
                            .FirstOrDefault(e => e.Attribute("name")?.Value == fieldName);
                        if (fieldElement != null)
                        {
                            row.Cells["StartCell"].Value = fieldElement.Attribute("StartCell")?.Value;
                            row.Cells["EndCell"].Value = fieldElement.Attribute("EndCell")?.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading mapping config: " + ex.Message,
                    "Config Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveMappingConfig()
        {
            try
            {
                XDocument doc = new XDocument(new XElement("Mapping"));
                foreach (DataGridViewRow row in dataGridViewMapping.Rows)
                {
                    if (row.IsNewRow) continue;
                    string fieldName = row.Cells["Field"].Value?.ToString();
                    string startCell = row.Cells["StartCell"].Value?.ToString();
                    string endCell = row.Cells["EndCell"].Value?.ToString();
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        XElement fieldElement = new XElement("Field",
                            new XAttribute("name", fieldName),
                            new XAttribute("StartCell", startCell ?? string.Empty),
                            new XAttribute("EndCell", endCell ?? string.Empty));
                        doc.Root.Add(fieldElement);
                    }
                }
                doc.Save(ConfigFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving mapping config: " + ex.Message,
                    "Config Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
