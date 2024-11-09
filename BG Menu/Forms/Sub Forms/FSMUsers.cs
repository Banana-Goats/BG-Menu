using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class FSMUsers : Form
    {
        private string connectionString = "Server=10.100.230.6:30015;UserID=ELLIOTRENNER;Password=Drop-Local-Poet-Knife-5";
        private DataTable dataTable;

        public FSMUsers()
        {
            string hanaClientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HANA_Client_Dlls");
            Environment.SetEnvironmentVariable("PATH", hanaClientPath + ";" + Environment.GetEnvironmentVariable("PATH"));
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection to SAP HANA database was successful.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to connect to SAP HANA database: " + ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExecuteAndDisplayQuery(object sender, EventArgs e)
        {

            var excludedUserCodes = new HashSet<string>
            {                
                "AdamA",
                "DanielC",
                "manager",
                "MuraliY",
                "B1i",
                "AndyW",
                "BobL",
                "ChrisC",
                "ChrisR",
                "CraigS",
                "DannyS",
                "DaveF",
                "GaryH",
                "GaryT",
                "GlennS",
                "IanBO",
                "IanN",                
                "JamieF",
                "JonathanW",
                "KyleN",
                "LiamB",
                "MasonS",
                "MatM",
                "MatthewR",
                "MickE",
                "MikeJ",
                "StephanH",
                "ThomasS",
                "ZacC"
            };

            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"",                                    
                                    'Ableworld UK' AS ""Company"" 
                                FROM ""SBO_AWUK_NEWLIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'SJLK' AS ""Company"" 
                                FROM ""SBO_SJLK_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'Southampton' AS ""Company"" 
                                FROM ""SBO_JCS_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'JSCD' AS ""Company"" 
                                FROM ""SBO_JSCD_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'AMD' AS ""Company"" 
                                FROM ""SBO_AMD_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'GRMR' AS ""Company"" 
                                FROM ""SBO_GRMR_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'Mobility GB' AS ""Company"" 
                                FROM ""SBO_MGB_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'
    
                                UNION ALL
    
                                SELECT 
                                    T0.""USER_CODE"", 
                                    T0.""U_NAME"", 
                                    T0.""U_Eng"", 
                                    T0.""U_COR_PLANNABLE"", 
                                    'AWG Southwest' AS ""Company"" 
                                FROM ""SBO_AWG_LIVE"".""OUSR"" T0 
                                WHERE T0.""U_Eng"" ='Y' OR T0.""U_COR_PLANNABLE"" ='Y'
                                AND T0.""Locked"" = 'N'";


                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataTable.Columns["USER_CODE"].ColumnName = "User Code";
                    dataTable.Columns["U_NAME"].ColumnName = "User Name";
                    dataTable.Columns["U_Eng"].ColumnName = "Active";
                    dataTable.Columns["U_COR_PLANNABLE"].ColumnName = "Plannable";
                    dataTable.Columns["Company"].ColumnName = "Company Name";

                    IEnumerable<DataRow> queryResult = dataTable.AsEnumerable()
                .Where(row => !excludedUserCodes.Contains(row.Field<string>("User Code")));  // Exclude users based on User Code

                    // Check if the checkbox is checked for grouping logic
                    if (toggleSlider1.Checked)
                    {
                        // Group by User Code, prioritizing "Ableworld UK"
                        queryResult = queryResult
                            .GroupBy(row => row.Field<string>("User Code"))
                            .Select(g =>
                                g.OrderByDescending(row => row.Field<string>("Company Name") == "Ableworld UK") // Prioritize Ableworld UK
                                .ThenBy(row => row.Field<string>("Company Name")) // Sort by Company Name if not Ableworld UK
                                .First());
                    }

                    // Apply sorting logic regardless of grouping
                    var finalUsers = queryResult
                        .OrderBy(row => row.Field<string>("Company Name") == "Ableworld UK" ? 0 : 1)  // Ensure Ableworld UK is at the top
                        .ThenBy(row => row.Field<string>("Company Name"))  // Sort the rest of the companies alphabetically
                        .ThenBy(row => row.Field<string>("User Code"))     // Sort by User Code within each company
                        .CopyToDataTable();

                    // Bind the final DataTable (with or without grouping) to the first DataGridView
                    dataGridView1.DataSource = finalUsers;

                    // Get the cost and discount values from the text boxes
                    decimal cost = 0;
                    decimal discount = 0;

                    if (!decimal.TryParse(textBoxCost.Text, out cost))
                    {
                        MessageBox.Show("Please enter a valid cost.");
                        return;
                    }

                    if (!decimal.TryParse(textBoxDiscount.Text, out discount))
                    {
                        MessageBox.Show("Please enter a valid discount.");
                        return;
                    }

                    // Calculate the discounted cost
                    decimal discountedCost = cost * ((100 - discount) / 100);

                    // Group the filtered data (finalUsers) by "Company Name" and calculate the total cost for each company
                    var companyTotals = from row in finalUsers.AsEnumerable()
                                        group row by row.Field<string>("Company Name") into grp
                                        select new
                                        {
                                            CompanyName = grp.Key,
                                            Count = grp.Count(),
                                            TotalCost = grp.Count() * discountedCost
                                        };

                    // Create a new DataTable to hold the summary data
                    DataTable summaryTable = new DataTable();
                    summaryTable.Columns.Add("Company Name", typeof(string));
                    summaryTable.Columns.Add("Count", typeof(int));
                    summaryTable.Columns.Add("Total Cost", typeof(string));  // Note: We store as string for formatting

                    // Populate the summary DataTable with formatted "Total Cost"
                    foreach (var item in companyTotals)
                    {
                        string formattedTotalCost = $"£{item.TotalCost:N2}";
                        summaryTable.Rows.Add(item.CompanyName, item.Count, formattedTotalCost);
                    }

                    // Bind the summary DataTable to the second DataGridView
                    dataGridView2.DataSource = summaryTable;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Ensure the dataTable is not null
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("No data available to transform. Please ensure data is loaded first.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get unique company names from the original DataTable
            var uniqueCompanies = dataTable.AsEnumerable()
                                           .Select(row => row.Field<string>("Company Name"))
                                           .Distinct()
                                           .ToList();

            // Get all unique user codes
            var uniqueUserCodes = dataTable.AsEnumerable()
                                           .Select(row => row.Field<string>("User Code"))
                                           .Distinct()
                                           .ToList();

            // Create a new DataTable where each column is a company (User Code column is no longer included)
            DataTable pivotTable = new DataTable();

            // Add a column for each company
            foreach (var company in uniqueCompanies)
            {
                pivotTable.Columns.Add(company, typeof(string));  // Column name is the company name
            }

            // Populate the new pivot table
            foreach (var userCode in uniqueUserCodes)
            {
                // Create a new row to represent user codes in their companies
                DataRow newRow = pivotTable.NewRow();

                // Fill each company column based on the original data
                foreach (var company in uniqueCompanies)
                {
                    // Check if the user is part of this company
                    var userInCompany = dataTable.AsEnumerable()
                        .FirstOrDefault(row => row.Field<string>("User Code") == userCode && row.Field<string>("Company Name") == company);

                    if (userInCompany != null)
                    {
                        newRow[company] = userCode;  // Fill the cell with the User Code
                    }
                    else
                    {
                        newRow[company] = "";  // Leave the cell blank if the user is not in this company
                    }
                }

                // Add the new row to the pivot table
                pivotTable.Rows.Add(newRow);
            }

            // Bind the new pivot table to the DataGridView (User Code column is omitted)
            dataGridView1.DataSource = pivotTable;
        }
    }
}
