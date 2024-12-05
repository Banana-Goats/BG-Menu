using BG_Menu.Class.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class PaymentDeviceManager : Form
    {
        private RoundedCorners roundedCorners;
        private string connectionString = "Server=Bananagoats.co.uk;Database=Ableworld;User Id=Elliot;Password=1234;";
        private int? existingIndexID = null;

        public PaymentDeviceManager()
        {
            InitializeComponent();
            var roundedCorners = new RoundedCorners(this, 70, 3, Color.Yellow);
            var LoginDraggable = new Draggable(this, this);

            if (!existingIndexID.HasValue)
            {
                this.Text = "Add New Payment Device"; // Change form title to indicate add mode
                btnDelete.Enabled = false; // Disable the Delete button in Add mode
            }

        }

        public PaymentDeviceManager(int indexID) : this()
        {
            existingIndexID = indexID;
            LoadExistingData();
            this.Text = "Edit Payment Device"; // Change form title to indicate edit mode
            btnDelete.Enabled = true;
        }

        private void LoadExistingData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
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
                    PCIDSSDate,
                    PCIDSSVersion,
                    PCIDSSPassword
                FROM PaymentDevices
                WHERE IndexID = @IndexID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IndexID", existingIndexID.Value);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate form fields with existing data
                                txtMerchantID.Text = reader["MerchantID"].ToString();
                                cmbMerchant.SelectedItem = reader["Merchant"].ToString();
                                txtTID.Text = reader["TID"] != DBNull.Value ? reader["TID"].ToString() : string.Empty;
                                txtPTID.Text = reader["PTID"] != DBNull.Value ? reader["PTID"].ToString() : string.Empty;
                                txtDevice.Text = reader["Device"] != DBNull.Value ? reader["Device"].ToString() : string.Empty;
                                txtSerialNumber.Text = reader["SerialNumber"] != DBNull.Value ? reader["SerialNumber"].ToString() : string.Empty;
                                txtCompany.Text = reader["Company"] != DBNull.Value ? reader["Company"].ToString() : string.Empty;
                                txtAssignedUser.Text = reader["AssignedUser"] != DBNull.Value ? reader["AssignedUser"].ToString() : string.Empty;
                                txtDepartmentStore.Text = reader["DepartmentStore"] != DBNull.Value ? reader["DepartmentStore"].ToString() : string.Empty;

                                if (reader["PCIDSSDate"] != DBNull.Value)
                                    dtpPCIDSSDate.Value = Convert.ToDateTime(reader["PCIDSSDate"]);

                                txtPCIDSSVersion.Text = reader["PCIDSSVersion"] != DBNull.Value ? reader["PCIDSSVersion"].ToString() : string.Empty;
                                txtPCIDSSPassword.Text = reader["PCIDSSPassword"] != DBNull.Value ? reader["PCIDSSPassword"].ToString() : string.Empty;
                                
                            }
                            else
                            {
                                MessageBox.Show("Selected payment device not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment device data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Retrieve values directly from control variables
            string merchantID = txtMerchantID.Text.Trim();
            string merchant = cmbMerchant.SelectedItem?.ToString();
            string tid = txtTID.Text.Trim();
            string ptid = txtPTID.Text.Trim();
            string device = txtDevice.Text.Trim();
            string serialNumber = txtSerialNumber.Text.Trim();
            string company = txtCompany.Text.Trim();
            string assignedUser = txtAssignedUser.Text.Trim();
            string departmentStore = txtDepartmentStore.Text.Trim();
            string pcidssVersion = txtPCIDSSVersion.Text.Trim();
            string pcidssPassword = txtPCIDSSPassword.Text.Trim();
            DateTime pcidssDate = dtpPCIDSSDate.Value.Date; // Retrieve the selected date

            // Revised validation: Only Merchant is required
            if (string.IsNullOrEmpty(merchant))
            {
                MessageBox.Show("Merchant is a required field.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbMerchant.Focus();
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query;
                    if (existingIndexID.HasValue)
                    {
                        // Edit mode: UPDATE existing record using IndexID
                        query = @"
                    UPDATE PaymentDevices
                    SET 
                        Merchant = @Merchant,
                        TID = @TID,
                        PTID = @PTID,
                        Device = @Device,
                        SerialNumber = @SerialNumber,
                        Company = @Company,
                        AssignedUser = @AssignedUser,
                        DepartmentStore = @DepartmentStore,
                        PCIDSSDate = @PCIDSSDate,
                        PCIDSSVersion = @PCIDSSVersion,
                        PCIDSSPassword = @PCIDSSPassword
                    WHERE IndexID = @IndexID";
                    }
                    else
                    {
                        // Add mode: INSERT new record
                        query = @"
                    INSERT INTO PaymentDevices (
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
                    )
                    VALUES (
                        @Merchant,
                        @TID,
                        @PTID,
                        @Device,
                        @SerialNumber,
                        @Company,
                        @AssignedUser,
                        @DepartmentStore,
                        @PCIDSSDate,
                        @PCIDSSVersion,
                        @PCIDSSPassword
                    )";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Add parameters and their values
                        if (existingIndexID.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@IndexID", existingIndexID.Value);
                        }

                        cmd.Parameters.AddWithValue("@Merchant", merchant);
                        cmd.Parameters.AddWithValue("@TID", string.IsNullOrEmpty(tid) ? (object)DBNull.Value : tid);
                        cmd.Parameters.AddWithValue("@PTID", string.IsNullOrEmpty(ptid) ? (object)DBNull.Value : ptid);
                        cmd.Parameters.AddWithValue("@Device", string.IsNullOrEmpty(device) ? (object)DBNull.Value : device);
                        cmd.Parameters.AddWithValue("@SerialNumber", string.IsNullOrEmpty(serialNumber) ? (object)DBNull.Value : serialNumber);
                        cmd.Parameters.AddWithValue("@Company", string.IsNullOrEmpty(company) ? (object)DBNull.Value : company);
                        cmd.Parameters.AddWithValue("@AssignedUser", string.IsNullOrEmpty(assignedUser) ? (object)DBNull.Value : assignedUser);
                        cmd.Parameters.AddWithValue("@DepartmentStore", string.IsNullOrEmpty(departmentStore) ? (object)DBNull.Value : departmentStore);
                        cmd.Parameters.AddWithValue("@PCIDSSDate", pcidssDate);
                        cmd.Parameters.AddWithValue("@PCIDSSVersion", string.IsNullOrEmpty(pcidssVersion) ? (object)DBNull.Value : pcidssVersion);
                        cmd.Parameters.AddWithValue("@PCIDSSPassword", string.IsNullOrEmpty(pcidssPassword) ? (object)DBNull.Value : pcidssPassword);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string message = existingIndexID.HasValue
                                ? "Payment device updated successfully."
                                : "Payment device added successfully.";

                            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Close the form after successful operation
                        }
                        else
                        {
                            string message = existingIndexID.HasValue
                                ? "Failed to update payment device."
                                : "Failed to add payment device.";

                            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string operation = existingIndexID.HasValue ? "updating" : "adding";
                MessageBox.Show($"Error {operation} payment device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Ensure the form is in Edit mode
            if (!existingIndexID.HasValue)
            {
                MessageBox.Show("Cannot delete a record that hasn't been saved yet.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm deletion with the user
            var confirmResult = MessageBox.Show("Are you sure you want to delete this payment device?",
                                                 "Confirm Deletion",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Define the DELETE query using IndexID
                        string deleteQuery = "DELETE FROM PaymentDevices WHERE IndexID = @IndexID";

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@IndexID", existingIndexID.Value);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Payment device deleted successfully.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK; // Indicate success to the calling form
                                this.Close(); // Close the manager form
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the payment device.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting payment device: {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
