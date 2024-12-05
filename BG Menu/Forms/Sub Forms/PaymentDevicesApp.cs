using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class PaymentDevicesApp : Form
    {
        private string connectionString = "Server=Bananagoats.co.uk;Database=Ableworld;User Id=Elliot;Password=1234;";
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;

        public PaymentDevicesApp(string currentUsername)
        {
            InitializeComponent();

            this.Text = $"Payment Device Managment - {currentUsername}";

            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT    
                    IndexID,
                    MerchantID,
                    Merchant,
                    TID,
                    PTID,
                    Device,
                    SerialNumber,
                    Company,
                    AssignedUser,
                    DepartmentStore,
                    PCIDSSDate,
                    PCIDSSVersion,
                    PCIDSSPassword
                FROM PaymentDevices";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    dataTable = new DataTable();

                    // Disable constraints for the DataTable
                    dataTable.Constraints.Clear();

                    dataAdapter.Fill(dataTable);

                    // Bind the DefaultView of the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable.DefaultView;


                    ConfigureMerchantColumn(); // Dynamically configure columns
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void ConfigureMerchantColumn()
        {
            if (dataGridView1.Columns["IndexID"] != null)
            {
                dataGridView1.Columns["IndexID"].Visible = false;
            }

            // Remove the default Merchant column if it exists
            if (dataGridView1.Columns["Merchant"] != null)
            {
                dataGridView1.Columns.Remove("Merchant");
            }

            // Create a new ComboBox column for the Merchant field
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Merchant",
                DataPropertyName = "Merchant", // Bind to the Merchant column in the DataTable
                Name = "Merchant", // Ensure the name matches the original column
                DataSource = new List<string> { "Elavon", "Worldpay" }, // Define options
                FlatStyle = FlatStyle.Flat // Optional: makes the ComboBox appear more modern
            };

            // Insert the ComboBox column in the correct position
            dataGridView1.Columns.Insert(2, comboBoxColumn); // Position 2 corresponds to the original "Merchant" column
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Use the same query as in LoadData
                    dataAdapter = new SqlDataAdapter(@"
                SELECT 
                    MerchantID,
                    Merchant,
                    TID,
                    PTID,
                    Device,
                    SerialNumber,
                    Company,
                    AssignedUser,
                    DepartmentStore,
                    PCIDSSVersion,
                    PCIDSSPassword
                FROM PaymentDevices", connection);

                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                    // Update changes in the DataTable back to the database
                    dataAdapter.Update(dataTable);

                    MessageBox.Show("Changes saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dataTable == null) return; // Ensure the data is loaded

            string searchValue = txtSearch.Text.Trim().Replace("'", "''"); // Sanitize input
            if (string.IsNullOrEmpty(searchValue))
            {
                dataTable.DefaultView.RowFilter = ""; // Clear filter
            }
            else
            {
                // Apply a filter using wildcards across all columns
                dataTable.DefaultView.RowFilter = $@"            
            Convert(MerchantID, 'System.String') LIKE '%{searchValue}%' OR
            Merchant LIKE '%{searchValue}%' OR
            TID LIKE '%{searchValue}%' OR
            PTID LIKE '%{searchValue}%' OR
            Company LIKE '%{searchValue}%' OR
            AssignedUser LIKE '%{searchValue}%' OR
            DepartmentStore LIKE '%{searchValue}%' OR
            Device LIKE '%{searchValue}%'";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Open PaymentDeviceManager in add mode
            using (PaymentDeviceManager manager = new PaymentDeviceManager())
            {
                manager.ShowDialog(); // Open as a modal dialog
                LoadData(); // Refresh data after the dialog is closed
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user double-clicked on a valid row
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Retrieve IndexID from the selected row
                if (selectedRow.Cells["IndexID"].Value != null && int.TryParse(selectedRow.Cells["IndexID"].Value.ToString(), out int indexID))
                {
                    // Open PaymentDeviceManager in edit mode using IndexID
                    using (PaymentDeviceManager manager = new PaymentDeviceManager(indexID))
                    {
                        manager.ShowDialog(); // Open as a modal dialog
                        LoadData(); // Refresh data after the dialog is closed
                    }
                }
                else
                {
                    MessageBox.Show("Invalid IndexID selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
