using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BG_Menu.Forms.MIDS
{
    public partial class UniqueMerchantsForm : Form
    {
        private string connectionString;
        private DataTable dataTable;

        public UniqueMerchantsForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;

            ConfigureDataGridView();
            LoadUniqueMerchants();            
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;

            // Existing columns...
            DataGridViewTextBoxColumn merchantIdColumn = new DataGridViewTextBoxColumn
            {
                Name = "MerchantID",
                HeaderText = "Merchant ID",
                DataPropertyName = "MerchantID",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            DataGridViewTextBoxColumn merchantColumn = new DataGridViewTextBoxColumn
            {
                Name = "Merchant",
                HeaderText = "Merchant",
                DataPropertyName = "Merchant",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            DataGridViewTextBoxColumn pciDssDateColumn = new DataGridViewTextBoxColumn
            {
                Name = "PCIDSSDate",
                HeaderText = "PCI DSS Date",
                DataPropertyName = "PCIDSSDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            DataGridViewTextBoxColumn pciDssVersionColumn = new DataGridViewTextBoxColumn
            {
                Name = "PCIDSSVersion",
                HeaderText = "PCI DSS Version",
                DataPropertyName = "PCIDSSVersion",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            DataGridViewTextBoxColumn pciDssPasswordColumn = new DataGridViewTextBoxColumn
            {
                Name = "PCIDSSPassword",
                HeaderText = "PCI DSS Password",
                DataPropertyName = "PCIDSSPassword",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            DataGridViewTextBoxColumn companyColumn = new DataGridViewTextBoxColumn
            {
                Name = "Company",
                HeaderText = "Company",
                DataPropertyName = "Company",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            // New Aggregated Columns:
            DataGridViewTextBoxColumn assignedUserColumn = new DataGridViewTextBoxColumn
            {
                Name = "AssignedUser",
                HeaderText = "Assigned User",
                DataPropertyName = "AssignedUser",
                ReadOnly = true, // since these are aggregated values
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            DataGridViewTextBoxColumn locationColumn = new DataGridViewTextBoxColumn
            {
                Name = "Location",
                HeaderText = "Location",
                DataPropertyName = "Location",
                ReadOnly = true, // aggregated
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            // Add all columns to the DataGridView
            dataGridView1.Columns.AddRange(
                merchantIdColumn,
                merchantColumn,
                pciDssDateColumn,
                pciDssVersionColumn,
                pciDssPasswordColumn,
                companyColumn,
                assignedUserColumn,
                locationColumn
            );

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void LoadUniqueMerchants()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT 
                        p.MerchantID,
                        MAX(p.Merchant) AS Merchant,
                        MAX(p.Company) AS Company,
                        MAX(p.PCIDSSDate) AS PCIDSSDate,
                        MAX(p.PCIDSSVersion) AS PCIDSSVersion,
                        MAX(p.PCIDSSPassword) AS PCIDSSPassword,
                        (
                           SELECT STRING_AGG(t.AssignedUser, ', ')
                           FROM (
                               SELECT DISTINCT AssignedUser 
                               FROM PaymentDevices 
                               WHERE MerchantID = p.MerchantID
                           ) t
                        ) AS AssignedUser,
                        (
                           SELECT STRING_AGG(t.DepartmentStore, ', ')
                           FROM (
                               SELECT DISTINCT DepartmentStore 
                               FROM PaymentDevices 
                               WHERE MerchantID = p.MerchantID
                           ) t
                        ) AS Location
                    FROM PaymentDevices p
                    GROUP BY p.MerchantID";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable; // Bind the DataTable to the DataGridView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading unique merchants: {ex.Message}");
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row.RowState == DataRowState.Modified) // Check if the row has been modified
                        {
                            string query = @"
                        UPDATE PaymentDevices
                        SET 
                            Merchant = @Merchant,
                            Company = @Company,
                            PCIDSSDate = @PCIDSSDate,
                            PCIDSSVersion = @PCIDSSVersion,
                            PCIDSSPassword = @PCIDSSPassword
                        WHERE MerchantID = @MerchantID";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Merchant
                                command.Parameters.AddWithValue("@Merchant", row["Merchant"] ?? DBNull.Value);

                                // Company
                                command.Parameters.AddWithValue("@Company", row["Company"] ?? DBNull.Value);

                                // PCI DSS Date
                                command.Parameters.AddWithValue("@PCIDSSDate", row["PCIDSSDate"] ?? DBNull.Value);

                                // PCI DSS Version
                                command.Parameters.AddWithValue("@PCIDSSVersion", row["PCIDSSVersion"] ?? DBNull.Value);

                                // PCI DSS Password
                                command.Parameters.AddWithValue("@PCIDSSPassword", row["PCIDSSPassword"] ?? DBNull.Value);

                                // MerchantID (WHERE)
                                command.Parameters.AddWithValue("@MerchantID", row["MerchantID"]);

                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the data after saving
                    LoadUniqueMerchants();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column is the PCIDSSDate column
            if (dataGridView1.Columns[e.ColumnIndex].Name == "PCIDSSDate" && e.Value != null)
            {
                if (DateTime.TryParse(e.Value.ToString(), out DateTime pciDssDate))
                {
                    TimeSpan age = DateTime.Now - pciDssDate;
                    double daysSince = age.TotalDays;

                    // Calculate the gradient
                    Color cellColor = GetGradientColor(daysSince);

                    e.CellStyle.BackColor = cellColor;
                    e.CellStyle.ForeColor = Color.Black; // Optional: Ensure text remains visible
                }
            }
        }

        private Color GetGradientColor(double daysSince)
        {
            // Days are scaled between 0 (green) and 365 (red)
            const double maxDays = 365.0;

            // Clamp the value between 0 and 1
            double proportion = Math.Min(Math.Max(daysSince / maxDays, 0.0), 1.0);

            // Calculate red, green, and blue components for the gradient
            int red = (int)(proportion * 255);              // Ranges from 0 to 255
            int green = (int)((1.0 - proportion) * 255);    // Ranges from 255 to 0
            int blue = 0;                                   // No blue for simplicity

            return Color.FromArgb(red, green, blue);
        }
    }
}
