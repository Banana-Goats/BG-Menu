using BG_Menu.Class.Design;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class PaymentDeviceManager : Form
    {
        private string currentUsername;
        private RoundedCorners roundedCorners;
        string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
        private int? existingIndexID = null;

        // Fields to store original values for edit operations
        private string originalMerchantID;
        private string originalMerchant;
        private string originalTID;
        private string originalPTID;
        private string originalDevice;
        private string originalSerialNumber;
        private string originalCompany;
        private string originalAssignedUser;
        private string originalDepartmentStore;
        private DateTime? originalPCIDSSDate;
        private string originalPCIDSSVersion;
        private string originalPCIDSSPassword;

        // Default constructor for add mode
        public PaymentDeviceManager(string username)
        {
            InitializeComponent();
            currentUsername = username; // Assign the passed username
            roundedCorners = new RoundedCorners(this, 70, 3, Color.Yellow);
            var LoginDraggable = new Draggable(this, this);

            InitializeForm(); // Common initialization
        }

        // Overloaded constructor for edit mode using IndexID
        public PaymentDeviceManager(int indexID, string username) : this(username)
        {
            existingIndexID = indexID;
            LoadExistingData();
            this.Text = "Edit Payment Device"; // Change form title to indicate edit mode
            btnDelete.Enabled = true; // Enable the Delete button in Edit mode
            btnSave.Text = "Edit Data";
        }

        // Common initialization method
        private void InitializeForm()
        {
            // Set default selections and dates
            if (cmbMerchant.Items.Count > 0)
                cmbMerchant.SelectedIndex = 0;

            dtpPCIDSSDate.Value = DateTime.Today;

            if (!existingIndexID.HasValue)
            {
                this.Text = "Add New Payment Device"; // Change form title to indicate add mode
                btnDelete.Enabled = false; // Disable the Delete button in Add mode
                btnSave.Text = "Add Data";
            }
        }

        // Method to load existing data into the form fields
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
                                // MerchantID
                                var mId = reader["MerchantID"].ToString();
                                txtMerchantID.Text = mId;
                                originalMerchantID = mId;

                                // Merchant
                                var merch = reader["Merchant"].ToString();
                                cmbMerchant.SelectedItem = merch;
                                originalMerchant = merch;

                                // TID
                                var tid = reader["TID"] != DBNull.Value ? reader["TID"].ToString() : string.Empty;
                                txtTID.Text = tid;
                                originalTID = tid;

                                // PTID
                                var ptid = reader["PTID"] != DBNull.Value ? reader["PTID"].ToString() : string.Empty;
                                txtPTID.Text = ptid;
                                originalPTID = ptid;

                                // Device
                                var dev = reader["Device"] != DBNull.Value ? reader["Device"].ToString() : string.Empty;
                                txtDevice.Text = dev;
                                originalDevice = dev;

                                // SerialNumber
                                var sn = reader["SerialNumber"] != DBNull.Value ? reader["SerialNumber"].ToString() : string.Empty;
                                txtSerialNumber.Text = sn;
                                originalSerialNumber = sn;

                                // Company
                                var comp = reader["Company"].ToString();
                                txtCompany.Text = comp;
                                originalCompany = comp;

                                // AssignedUser
                                var user = reader["AssignedUser"].ToString();
                                txtAssignedUser.Text = user;
                                originalAssignedUser = user;

                                // DepartmentStore
                                var dept = reader["DepartmentStore"].ToString();
                                txtDepartmentStore.Text = dept;
                                originalDepartmentStore = dept;

                                // PCIDSSDate
                                if (reader["PCIDSSDate"] != DBNull.Value)
                                {
                                    var dt = Convert.ToDateTime(reader["PCIDSSDate"]);
                                    dtpPCIDSSDate.Value = dt;
                                    originalPCIDSSDate = dt;
                                }
                                else
                                {
                                    dtpPCIDSSDate.Value = DateTime.Today;
                                    originalPCIDSSDate = null;
                                }

                                // PCIDSSVersion
                                var ver = reader["PCIDSSVersion"].ToString();
                                txtPCIDSSVersion.Text = ver;
                                originalPCIDSSVersion = ver;

                                // PCIDSSPassword
                                var pwd = reader["PCIDSSPassword"].ToString();
                                txtPCIDSSPassword.Text = pwd;
                                originalPCIDSSPassword = pwd;
                            }
                            else
                            {
                                MessageBox.Show("Selected payment device not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }
                        }
                    }

                    // Query the last change details from the PaymentDevicesAudit table
                    string auditQuery = @"
                SELECT TOP 1
                    ChangedBy,
                    ChangeDate
                FROM PaymentDevicesAudit
                WHERE IndexID = @IndexID
                ORDER BY ChangeDate DESC";

                    using (SqlCommand cmdAudit = new SqlCommand(auditQuery, connection))
                    {
                        cmdAudit.Parameters.AddWithValue("@IndexID", existingIndexID.Value);

                        using (SqlDataReader auditReader = cmdAudit.ExecuteReader())
                        {
                            if (auditReader.Read())
                            {
                                txtChangedBy.Text = auditReader["ChangedBy"].ToString();
                                txtChangeDate.Text = auditReader["ChangeDate"] != DBNull.Value
                                    ? Convert.ToDateTime(auditReader["ChangeDate"]).ToString("dd/MM/yy HH:mm")
                                    : "N/A";
                            }
                            else
                            {
                                txtChangedBy.Text = "N/A";
                                txtChangeDate.Text = "N/A";
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


        // Method to insert audit logs
        private void InsertAuditLog(int indexID, string operationType, string fieldName, string oldValue, string newValue)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string auditQuery = @"
                        INSERT INTO PaymentDevicesAudit (
                            IndexID,
                            OperationType,
                            FieldName,
                            OldValue,
                            NewValue,
                            ChangedBy,
                            ChangeDate
                        )
                        VALUES (
                            @IndexID,
                            @OperationType,
                            @FieldName,
                            @OldValue,
                            @NewValue,
                            @ChangedBy,
                            @ChangeDate
                        )";

                    using (SqlCommand cmd = new SqlCommand(auditQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@IndexID", indexID);
                        cmd.Parameters.AddWithValue("@OperationType", operationType);
                        cmd.Parameters.AddWithValue("@FieldName", (object)fieldName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@OldValue", (object)oldValue ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NewValue", (object)newValue ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ChangedBy", currentUsername);
                        cmd.Parameters.AddWithValue("@ChangeDate", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it as per your application's requirement
                MessageBox.Show($"Error logging audit data: {ex.Message}", "Audit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to compare original and new values and log changes
        private void CompareAndLogChanges(int indexID)
        {
            // Compare each field and log changes
            if (originalMerchant != cmbMerchant.SelectedItem?.ToString())
            {
                InsertAuditLog(indexID, "Edit", "Merchant", originalMerchant, cmbMerchant.SelectedItem?.ToString());
            }

            if (originalTID != txtTID.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "TID", originalTID, txtTID.Text.Trim());
            }

            if (originalPTID != txtPTID.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "PTID", originalPTID, txtPTID.Text.Trim());
            }

            if (originalDevice != txtDevice.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "Device", originalDevice, txtDevice.Text.Trim());
            }

            if (originalSerialNumber != txtSerialNumber.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "SerialNumber", originalSerialNumber, txtSerialNumber.Text.Trim());
            }

            if (originalCompany != txtCompany.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "Company", originalCompany, txtCompany.Text.Trim());
            }

            if (originalAssignedUser != txtAssignedUser.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "AssignedUser", originalAssignedUser, txtAssignedUser.Text.Trim());
            }

            if (originalDepartmentStore != txtDepartmentStore.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "DepartmentStore", originalDepartmentStore, txtDepartmentStore.Text.Trim());
            }

            if (originalPCIDSSDate != dtpPCIDSSDate.Value.Date)
            {
                string oldDate = originalPCIDSSDate.HasValue ? originalPCIDSSDate.Value.ToString("yyyy-MM-dd") : "NULL";
                string newDate = dtpPCIDSSDate.Value.Date.ToString("yyyy-MM-dd");
                InsertAuditLog(indexID, "Edit", "PCIDSSDate", oldDate, newDate);
            }

            if (originalPCIDSSVersion != txtPCIDSSVersion.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "PCIDSSVersion", originalPCIDSSVersion, txtPCIDSSVersion.Text.Trim());
            }

            if (originalPCIDSSPassword != txtPCIDSSPassword.Text.Trim())
            {
                InsertAuditLog(indexID, "Edit", "PCIDSSPassword", originalPCIDSSPassword, txtPCIDSSPassword.Text.Trim());
            }

            // **Always log TID and PTID during edit operations, even if they haven't changed**
            InsertAuditLog(indexID, "Edit", "TID", originalTID, txtTID.Text.Trim());
            InsertAuditLog(indexID, "Edit", "PTID", originalPTID, txtPTID.Text.Trim());

            // **Always log MerchantID during edit operations, even if it hasn't changed**
            InsertAuditLog(indexID, "Edit", "MerchantID", originalMerchantID, txtMerchantID.Text.Trim());
        }

        // Method to log each field's initial value during Add operation
        private void LogAddOperation(int indexID)
        {
            InsertAuditLog(indexID, "Add", "MerchantID", null, txtMerchantID.Text.Trim());
            InsertAuditLog(indexID, "Add", "Merchant", null, cmbMerchant.SelectedItem?.ToString());
            InsertAuditLog(indexID, "Add", "TID", null, txtTID.Text.Trim());
            InsertAuditLog(indexID, "Add", "PTID", null, txtPTID.Text.Trim());
            InsertAuditLog(indexID, "Add", "Device", null, txtDevice.Text.Trim());
            InsertAuditLog(indexID, "Add", "SerialNumber", null, txtSerialNumber.Text.Trim());
            InsertAuditLog(indexID, "Add", "Company", null, txtCompany.Text.Trim());
            InsertAuditLog(indexID, "Add", "AssignedUser", null, txtAssignedUser.Text.Trim());
            InsertAuditLog(indexID, "Add", "DepartmentStore", null, txtDepartmentStore.Text.Trim());
            InsertAuditLog(indexID, "Add", "PCIDSSDate", null, dtpPCIDSSDate.Value.Date.ToString("yyyy-MM-dd"));
            InsertAuditLog(indexID, "Add", "PCIDSSVersion", null, txtPCIDSSVersion.Text.Trim());
            InsertAuditLog(indexID, "Add", "PCIDSSPassword", null, txtPCIDSSPassword.Text.Trim());
        }

        // Method to retrieve current record data for deletion logging
        private Dictionary<string, string> GetCurrentRecordData(int indexID)
        {
            var recordData = new Dictionary<string, string>();

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
                        cmd.Parameters.AddWithValue("@IndexID", indexID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                recordData.Add("MerchantID", reader["MerchantID"].ToString());
                                recordData.Add("Merchant", reader["Merchant"].ToString());
                                recordData.Add("TID", reader["TID"] != DBNull.Value ? reader["TID"].ToString() : string.Empty);
                                recordData.Add("PTID", reader["PTID"].ToString());
                                recordData.Add("Device", reader["Device"].ToString());
                                recordData.Add("SerialNumber", reader["SerialNumber"].ToString());
                                recordData.Add("Company", reader["Company"].ToString());
                                recordData.Add("AssignedUser", reader["AssignedUser"].ToString());
                                recordData.Add("DepartmentStore", reader["DepartmentStore"].ToString());
                                recordData.Add("PCIDSSDate", reader["PCIDSSDate"] != DBNull.Value ? Convert.ToDateTime(reader["PCIDSSDate"]).ToString("yyyy-MM-dd") : "NULL");
                                recordData.Add("PCIDSSVersion", reader["PCIDSSVersion"].ToString());
                                recordData.Add("PCIDSSPassword", reader["PCIDSSPassword"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving record data for deletion: {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return recordData;
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
                                MerchantID = @MerchantID,
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
                            )
                            VALUES (
                                @MerchantID,
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
                            ); 
                            SELECT CAST(scope_identity() AS int);"; // Retrieve the newly inserted IndexID
                    }

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Add parameters and their values
                        if (existingIndexID.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@IndexID", existingIndexID.Value);
                        }

                        cmd.Parameters.AddWithValue("@MerchantID", string.IsNullOrEmpty(merchantID) ? (object)DBNull.Value : merchantID);
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

                        int rowsAffected = 0;
                        int newIndexID = 0;

                        if (existingIndexID.HasValue)
                        {
                            // Execute UPDATE
                            rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Compare old and new values and log changes
                                CompareAndLogChanges(existingIndexID.Value);
                            }
                        }
                        else
                        {
                            // Execute INSERT and retrieve the new IndexID
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                newIndexID = Convert.ToInt32(result);
                                rowsAffected = 1;

                                if (rowsAffected > 0)
                                {
                                    // Log the addition of a new record
                                    InsertAuditLog(newIndexID, "Add", null, null, null);

                                    // Log each field's initial value as an addition
                                    LogAddOperation(newIndexID);
                                }
                            }
                        }

                        if (rowsAffected > 0)
                        {
                            string message = existingIndexID.HasValue
                                ? "Payment device updated successfully."
                                : "Payment device added successfully.";

                            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK; // Indicate success to the calling form
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
                    // Fetch current record data before deletion
                    var recordData = GetCurrentRecordData(existingIndexID.Value);

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
                                // Log the deletion for each field
                                foreach (var field in recordData)
                                {
                                    InsertAuditLog(existingIndexID.Value, "Delete", field.Key, field.Value, null);
                                }

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
