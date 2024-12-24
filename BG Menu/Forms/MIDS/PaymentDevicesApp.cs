using BG_Menu.Forms.MIDS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class PaymentDevicesApp : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private string currentUsername;

        public PaymentDevicesApp(string currentUsername)
        {
            InitializeComponent();
            this.currentUsername = currentUsername;

            this.Text = $"Payment Device Managment - {currentUsername}";

            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;

            LoadData();

            ConfigureDataGridViewAppearance();
        }

        private void ConfigureDataGridViewAppearance()
        {
            // Set the default row style
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Very light gray
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black; // Default text color

            // Set the alternating row style
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 240, 255); // Very light blue-gray
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black; // Default text color

            // Optional: Adjust font or selection styles
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10); // Set a clean font
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Select full rows for better visibility
        }

        private void LoadData(int? editedIndexID = null)
        {
            if (this.IsHandleCreated)
            {
                // Use BeginInvoke for smoother visual updates
                this.BeginInvoke((MethodInvoker)delegate
                {
                    LoadDataInternal(editedIndexID);
                });
            }
            else
            {
                // Call the internal load method directly if handle isn't created
                LoadDataInternal(editedIndexID);
            }
        }

        private void LoadDataInternal(int? editedIndexID)
        {
            int? scrollPosition = null;
            int? selectedRowIndex = null;

            // Preserve the current scroll position
            if (dataGridView1.FirstDisplayedScrollingRowIndex >= 0)
            {
                scrollPosition = dataGridView1.FirstDisplayedScrollingRowIndex;
            }

            // Preserve the currently selected row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedRowIndex = dataGridView1.SelectedRows[0].Index;
            }

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

                if (editedIndexID.HasValue)
                {
                    // Auto-select the updated row
                    var updatedRow = dataTable.Select($"IndexID = {editedIndexID.Value}").FirstOrDefault();
                    if (updatedRow != null)
                    {
                        int updatedRowIndex = dataTable.Rows.IndexOf(updatedRow);
                        dataGridView1.Rows[updatedRowIndex].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = updatedRowIndex;
                    }
                }
                else
                {
                    // Restore the scroll position
                    if (scrollPosition.HasValue)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = scrollPosition.Value;
                    }

                    // Restore the selected row
                    if (selectedRowIndex.HasValue && selectedRowIndex.Value < dataGridView1.Rows.Count)
                    {
                        dataGridView1.Rows[selectedRowIndex.Value].Selected = true;
                    }
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
            // Open PaymentDeviceManager in add mode with currentUsername
            using (PaymentDeviceManager manager = new PaymentDeviceManager(currentUsername))
            {
                var result = manager.ShowDialog(); // Open as a modal dialog

                if (result == DialogResult.OK)
                {
                    LoadData(); // Refresh data after the dialog is closed
                }
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
                    using (PaymentDeviceManager manager = new PaymentDeviceManager(indexID, currentUsername))
                    {
                        var result = manager.ShowDialog(); // Open as a modal dialog

                        if (result == DialogResult.OK)
                        {
                            // Reload data and auto-select the updated row
                            LoadData(indexID);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid IndexID selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Initialize the search form with the connection string
            SearchAssignmentsForm searchForm = new SearchAssignmentsForm(connectionString);

            // Show the search form as a dialog
            var result = searchForm.ShowDialog();

            // Optionally handle the dialog result
            if (result == DialogResult.OK)
            {
                // Search was executed; results are already displayed in the dialog
                // Additional actions can be performed here if needed
            }
        }

        private void btnUniqueMerchants_Click(object sender, EventArgs e)
        {
            // Open the UniqueMerchantsForm
            using (UniqueMerchantsForm uniqueMerchantsForm = new UniqueMerchantsForm(connectionString))
            {
                uniqueMerchantsForm.ShowDialog();
            }
        }
    }
}
