using Sap.Data.Hana;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BG_Menu.Data;
using BG_Menu.Class.Sales_Summary;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class Refunds : Form
    {
        private SalesRepository salesRepository;
        private DataTable dataTable;

        public Refunds()
        {
            string hanaClientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HANA_Client_Dlls");
            Environment.SetEnvironmentVariable("PATH", hanaClientPath + ";" + Environment.GetEnvironmentVariable("PATH"));

            InitializeComponent();
            salesRepository = GlobalInstances.SalesRepository;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string query = BuildQuery();
            ExecuteAndDisplayQuery(query);
        }

        private string BuildQuery()
        {
            DateTime fromDate = dateTimePickerFrom.Value.Date;
            DateTime toDate = DateTime.Today;

            return $@"
                SELECT DISTINCT
                    T0.""DocEntry"", T0.""DocDate"", T2.""WhsName"", T0.""CardCode"", T0.""CardName"",
                    T1.""ItemCode"", T1.""Dscription"", T1.""LineTotal"", T3.""SlpName""
                FROM ""SBO_AWUK_NEWLIVE"".""ORIN"" T0
                INNER JOIN ""SBO_AWUK_NEWLIVE"".""RIN1"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                INNER JOIN ""SBO_AWUK_NEWLIVE"".""OWHS"" T2 ON T1.""WhsCode"" = T2.""WhsCode""
                INNER JOIN ""SBO_AWUK_NEWLIVE"".""OSLP"" T3 ON T0.""SlpCode"" = T3.""SlpCode""
                WHERE T0.""CANCELED"" = 'N' 
                  AND T0.""DocDate"" BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                  AND T2.""WhsCode"" BETWEEN 'AA0001' AND 'AA1099'
                  AND (T1.""LineTotal"" > 250 OR T1.""LineTotal"" < -250)
                ORDER BY T2.""WhsName"", T0.""DocDate"", T3.""SlpName"";";
        }

        private void ExecuteAndDisplayQuery(string query)
        {
            try
            {
                // Delegate query execution to the repository
                dataTable = salesRepository.ExecuteHanaQuery(query);

                // Rename columns for better readability
                dataTable.Columns["DocEntry"].ColumnName = "Document Entry";
                dataTable.Columns["DocDate"].ColumnName = "Document Date";
                dataTable.Columns["WhsName"].ColumnName = "Warehouse Name";
                dataTable.Columns["CardCode"].ColumnName = "Customer Code";
                dataTable.Columns["CardName"].ColumnName = "Customer Name";
                dataTable.Columns["ItemCode"].ColumnName = "Item Code";
                dataTable.Columns["Dscription"].ColumnName = "Description";
                dataTable.Columns["LineTotal"].ColumnName = "Line Total";
                dataTable.Columns["SlpName"].ColumnName = "Salesperson";

                // Bind the data to the DataGridView
                dataGridViewResults.DataSource = dataTable;

                // Format the Line Total column
                if (dataGridViewResults.Columns["Line Total"] != null)
                {
                    var lineTotalColumn = dataGridViewResults.Columns["Line Total"];
                    lineTotalColumn.DefaultCellStyle.Format = "C2";
                    lineTotalColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Center align headers and cells
                foreach (DataGridViewColumn column in dataGridViewResults.Columns)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Auto-size columns
                dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MessageBox.Show("Query executed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button 1: Show Total Line Total by Warehouse
        private void btnWarehouseTotals_Click(object sender, EventArgs e)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("No data available. Run a query first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Warehouse mapping based on the provided data
            var warehouseMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Ableworld Birkenhead", "Birkenhead" },
                    { "Birkenhead Off-Line", "Birkenhead" },
                    { "Ableworld Burton", "Burton" },
                    { "Burton Off-Line", "Burton" },
                    { "Ableworld Chester", "Chester" },
                    { "Chester Off-Line", "Chester" },
                    { "Ableworld Congleton", "Congleton" },
                    { "Congleton Off-Line", "Congleton" },
                    { "Ableworld Cheltenham", "Cheltenham" },
                    { "Cheltenham Off-Line", "Cheltenham" },
                    { "Ableworld Crewe", "Crewe" },
                    { "Crewe Off-Line", "Crewe" },
                    { "Ableworld Darlington", "Darlington" },
                    { "Darlington Offline", "Darlington" },
                    { "Ableworld Gloucester", "Gloucester" },
                    { "Gloucester Offline", "Gloucester" },
                    { "Ableworld Hanley", "Hanley" },
                    { "Hanley Off-Line", "Hanley" },
                    { "Ableworld Lincoln", "Lincoln" },
                    { "Lincoln Offline", "Lincoln" },
                    { "Ableworld Llandudno", "Llandudno" },
                    { "Llandudno Off-Line", "Llandudno" },
                    { "Ableworld Nantwich", "Nantwich" },
                    { "Nantwich Off-Line", "Nantwich" },
                    { "Ableworld Newark", "Newark" },
                    { "Newark Offline", "Newark" },
                    { "Ableworld Newport", "Newport" },
                    { "Newport Off-Line", "Newport" },
                    { "Ableworld Northwich", "Northwich" },
                    { "Northwich Off-Line", "Northwich" },
                    { "Ableworld Oswestry", "Oswestry" },
                    { "Oswestry Off-Line", "Oswestry" },
                    { "Ableworld Queensferry", "Queensferry" },
                    { "Queensferry Off-line", "Queensferry" },
                    { "Ableworld Reading", "Reading" },
                    { "Reading Offline", "Reading" },
                    { "Ableworld Rhyl", "Rhyl" },
                    { "Rhyl Off-Line", "Rhyl" },
                    { "Ableworld Runcorn", "Runcorn" },
                    { "Runcorn Offline", "Runcorn" },
                    { "Ableworld Shrewsbury", "Shrewsbury" },
                    { "Shrewsbury Off-Line", "Shrewsbury" },
                    { "Ableworld Stafford", "Stafford" },
                    { "Stafford Off-Line", "Stafford" },
                    { "Ableworld Stockport", "Stockport" },
                    { "Stockport Off-Line", "Stockport" },
                    { "Ableworld Stockton", "Stockton" },
                    { "Stockton Offline", "Stockton" },
                    { "Ableworld Thatcham", "Thatcham" },
                    { "Thatcham Off-Line", "Thatcham" },
                    { "Ableworld Wrexham", "Wrexham" },
                    { "Wrexham Off-Line", "Wrexham" }
                };

            // Group by consolidated warehouse names
            var warehouseTotals = from row in dataTable.AsEnumerable()
                                  let warehouseName = row.Field<string>("Warehouse Name")
                                  let consolidatedName = warehouseMapping.ContainsKey(warehouseName)
                                      ? warehouseMapping[warehouseName]
                                      : warehouseName
                                  group row by consolidatedName into g
                                  select new
                                  {
                                      Warehouse = g.Key,
                                      TotalLineTotal = g.Sum(r => r["Line Total"] != DBNull.Value ? Convert.ToDecimal(r["Line Total"]) : 0)
                                  };

            // Create a DataTable to hold the consolidated results
            DataTable warehouseTable = new DataTable();
            warehouseTable.Columns.Add("Warehouse Name", typeof(string));
            warehouseTable.Columns.Add("Total Line Total", typeof(string));

            foreach (var item in warehouseTotals)
            {
                string formattedLineTotal = item.TotalLineTotal > 0 ? $"£{item.TotalLineTotal:N2}" : "£0.00";
                warehouseTable.Rows.Add(item.Warehouse, formattedLineTotal);
            }

            // Show the popup with the consolidated results
            ShowPopup("Warehouse Totals", warehouseTable);
        }

        // Button 2: Show Total Line Total by Salesperson
        private void btnSalespersonTotals_Click(object sender, EventArgs e)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("No data available. Run a query first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var salespersonTotals = from row in dataTable.AsEnumerable()
                                    group row by row.Field<string>("Salesperson") into g
                                    select new
                                    {
                                        Salesperson = g.Key,
                                        TotalLineTotal = g.Sum(r => Convert.ToDecimal(r["Line Total"]))
                                    };

            DataTable salespersonTable = new DataTable();
            salespersonTable.Columns.Add("Salesperson", typeof(string));
            salespersonTable.Columns.Add("Total Line Total", typeof(string));

            foreach (var item in salespersonTotals)
            {
                salespersonTable.Rows.Add(item.Salesperson, $"£{item.TotalLineTotal:N2}");
            }

            ShowPopup("Salesperson Totals", salespersonTable);
        }

        private void ShowPopup(string title, DataTable data)
        {
            Form popup = new Form
            {
                Text = title,
                Width = 600,
                Height = 600
            };

            DataGridView gridView = new DataGridView
            {
                DataSource = data,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Center align headers and cells
            foreach (DataGridViewColumn column in gridView.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            popup.Controls.Add(gridView);
            popup.ShowDialog();
        }
    }
}
