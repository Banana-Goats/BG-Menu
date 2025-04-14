using Google.Cloud.Firestore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace BG_Menu.Forms.Sub_Forms
{
    public partial class UserManagement : Form
    {
        private FirestoreDb firestoreDb;
        private bool isLoadingPermissions = false;
        private bool isLoadingStores = false;

        private ConcurrentQueue<Func<Task>> updateQueue = new ConcurrentQueue<Func<Task>>();
        private bool isProcessingQueue = false;

        string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

        private PopupForm popupForm; // Keep a reference to the popup form

        public UserManagement(FirestoreDb db)
        {
            InitializeComponent();

            firestoreDb = db;

            LoadUsers();
            SetupPermissionsGrid();
            SetupStoresGrid();
            LoadStoreAndFranchiseData();

            dataGridViewPermissions.CurrentCellDirtyStateChanged += DataGridViewPermissions_CurrentCellDirtyStateChanged;
            dataGridViewPermissions.CellValueChanged += DataGridViewPermissions_CellValueChanged;

            dataGridViewStoresFranchises.CurrentCellDirtyStateChanged += DataGridViewStoresFranchises_CurrentCellDirtyStateChanged;
            dataGridViewStoresFranchises.CellValueChanged += DataGridViewStoresFranchises_CellValueChanged;
        }

        #region Manage

        private async void LoadUsers()
        {
            try
            {
                var usersCollection = firestoreDb.Collection("BG-Users");
                var snapshot = await usersCollection.GetSnapshotAsync();

                foreach (var doc in snapshot.Documents)
                {
                    listBoxUsers.Items.Add(doc.Id);  // Add username to the list box
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupPermissionsGrid()
        {
            // Set up the DataGridView with two columns
            dataGridViewPermissions.Columns.Add("Permission", "Permission");
            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "HasPermission",
                HeaderText = "Has Permission",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            dataGridViewPermissions.Columns.Add(checkBoxColumn);

            // Populate the DataGridView with available permissions
            foreach (var permission in UserSession.AvailablePermissions)
            {
                dataGridViewPermissions.Rows.Add(permission, false);
            }
        }

        private void SetupStoresGrid()
        {
            // Clear existing columns first, if any
            dataGridViewStoresFranchises.Columns.Clear();

            // Add UK Store column and its associated Select column
            dataGridViewStoresFranchises.Columns.Add("UKStore", "UK Store");

            var checkBoxColumn1 = new DataGridViewCheckBoxColumn
            {
                Name = "SelectUKStore",
                HeaderText = "Select",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridViewStoresFranchises.Columns.Add(checkBoxColumn1);

            // Add Franchise Store column and its associated Select column
            dataGridViewStoresFranchises.Columns.Add("FranchiseStore", "Franchise Store");

            var checkBoxColumn2 = new DataGridViewCheckBoxColumn
            {
                Name = "SelectFranchiseStore",
                HeaderText = "Select",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridViewStoresFranchises.Columns.Add(checkBoxColumn2);

            // Add Franchise column and its associated Select column
            dataGridViewStoresFranchises.Columns.Add("Franchise", "Franchise");

            var checkBoxColumn3 = new DataGridViewCheckBoxColumn
            {
                Name = "SelectFranchise",
                HeaderText = "Select",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridViewStoresFranchises.Columns.Add(checkBoxColumn3);
        }

        private async void LoadStoreAndFranchiseData()
        {
            try
            {
                DocumentReference locationDoc = firestoreDb.Collection("Stores & Franchises").Document("Location");
                DocumentSnapshot snapshot = await locationDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var ukStores = snapshot.GetValue<List<string>>("UK Stores") ?? new List<string>();
                    var franchiseStores = snapshot.GetValue<List<string>>("Franchise Stores") ?? new List<string>();
                    var franchises = snapshot.GetValue<List<string>>("Franchises") ?? new List<string>();

                    PopulateStoreAndFranchiseGrid(ukStores, franchiseStores, franchises);
                }
                else
                {
                    MessageBox.Show("No data found for 'Locations' document.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load store and franchise data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateStoreAndFranchiseGrid(List<string> ukStores, List<string> franchiseStores, List<string> franchises)
        {
            dataGridViewStoresFranchises.Rows.Clear();

            int maxRows = Math.Max(ukStores.Count, Math.Max(franchiseStores.Count, franchises.Count));

            for (int i = 0; i < maxRows; i++)
            {
                string ukStore = i < ukStores.Count ? ukStores[i] : string.Empty;
                string franchiseStore = i < franchiseStores.Count ? franchiseStores[i] : string.Empty;
                string franchise = i < franchises.Count ? franchises[i] : string.Empty;

                // Add the store names and initialize the checkboxes as unchecked (false)
                dataGridViewStoresFranchises.Rows.Add(ukStore, false, franchiseStore, false, franchise, false);
            }
        }

        private async Task LoadUserStores(string username)
        {
            try
            {
                isLoadingStores = true;  // Prevent the event from firing during load

                // Clear all checkboxes first without altering text columns
                foreach (DataGridViewRow row in dataGridViewStoresFranchises.Rows)
                {
                    row.Cells["SelectUKStore"].Value = false;
                    row.Cells["SelectFranchiseStore"].Value = false;
                    row.Cells["SelectFranchise"].Value = false;
                }

                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(username);
                DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var userStores = snapshot.GetValue<List<string>>("Stores") ?? new List<string>();

                    foreach (DataGridViewRow row in dataGridViewStoresFranchises.Rows)
                    {
                        // Ensure correct checkbox column is paired with the right text column
                        string ukStore = row.Cells["UKStore"].Value?.ToString();
                        string franchiseStore = row.Cells["FranchiseStore"].Value?.ToString();
                        string franchise = row.Cells["Franchise"].Value?.ToString();

                        // Check the appropriate checkbox if the store is in the user's store list
                        if (!string.IsNullOrEmpty(ukStore) && userStores.Contains(ukStore))
                        {
                            row.Cells["SelectUKStore"].Value = true;
                        }
                        if (!string.IsNullOrEmpty(franchiseStore) && userStores.Contains(franchiseStore))
                        {
                            row.Cells["SelectFranchiseStore"].Value = true;
                        }
                        if (!string.IsNullOrEmpty(franchise) && userStores.Contains(franchise))
                        {
                            row.Cells["SelectFranchise"].Value = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load stores for user {username}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingStores = false;  // Allow event firing after load
            }
        }


        private void DataGridViewPermissions_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewPermissions.IsCurrentCellDirty)
            {
                // This commits the change immediately after a checkbox is clicked
                dataGridViewPermissions.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private async void DataGridViewPermissions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Only process the event if we are not loading permissions
            if (!isLoadingPermissions && e.RowIndex >= 0 && e.ColumnIndex == dataGridViewPermissions.Columns["HasPermission"].Index)
            {
                string selectedUser = listBoxUsers.SelectedItem?.ToString();
                if (selectedUser != null)
                {
                    string permission = dataGridViewPermissions.Rows[e.RowIndex].Cells["Permission"].Value?.ToString();
                    bool hasPermission = (bool)dataGridViewPermissions.Rows[e.RowIndex].Cells["HasPermission"].Value;

                    updateQueue.Enqueue(() => UpdateUserPermission(selectedUser, permission, hasPermission));
                    await ProcessQueue(); // Process the queue sequentially
                }
            }
        }

        private void DataGridViewStoresFranchises_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewStoresFranchises.IsCurrentCellDirty)
            {
                dataGridViewStoresFranchises.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private async void DataGridViewStoresFranchises_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!isLoadingStores && e.RowIndex >= 0)
            {
                string selectedUser = listBoxUsers.SelectedItem?.ToString();
                if (selectedUser != null)
                {
                    string storeName = null;

                    // Identify which store name is associated with the checked checkbox
                    if (e.ColumnIndex == dataGridViewStoresFranchises.Columns["SelectUKStore"].Index)
                    {
                        storeName = dataGridViewStoresFranchises.Rows[e.RowIndex].Cells["UKStore"].Value?.ToString();
                    }
                    else if (e.ColumnIndex == dataGridViewStoresFranchises.Columns["SelectFranchiseStore"].Index)
                    {
                        storeName = dataGridViewStoresFranchises.Rows[e.RowIndex].Cells["FranchiseStore"].Value?.ToString();
                    }
                    else if (e.ColumnIndex == dataGridViewStoresFranchises.Columns["SelectFranchise"].Index)
                    {
                        storeName = dataGridViewStoresFranchises.Rows[e.RowIndex].Cells["Franchise"].Value?.ToString();
                    }

                    if (!string.IsNullOrEmpty(storeName))
                    {
                        bool isChecked = (bool)dataGridViewStoresFranchises.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                        updateQueue.Enqueue(() => UpdateUserStore(selectedUser, storeName, isChecked));
                        await ProcessQueue(); // Process the queue sequentially
                    }
                }
            }
        }

        private async Task ProcessQueue()
        {
            if (isProcessingQueue)
                return;

            isProcessingQueue = true;

            while (updateQueue.TryDequeue(out var updateTask))
            {
                try
                {
                    await updateTask();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            isProcessingQueue = false;
        }

        private async void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedItem != null)
            {
                string selectedUser = listBoxUsers.SelectedItem.ToString();
                await LoadUserPermissions(selectedUser);
                await LoadUserStores(selectedUser);  // Load and display user-specific store selections
            }
        }

        private async Task LoadUserPermissions(string username)
        {
            try
            {
                isLoadingPermissions = true; // Set flag to true to prevent unwanted event firing

                // Clear previous selections
                foreach (DataGridViewRow row in dataGridViewPermissions.Rows)
                {
                    row.Cells["HasPermission"].Value = false;
                }

                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(username);
                DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var userPermissions = snapshot.GetValue<List<string>>("Permissions") ?? new List<string>();

                    foreach (DataGridViewRow row in dataGridViewPermissions.Rows)
                    {
                        string permission = row.Cells["Permission"].Value?.ToString();
                        if (!string.IsNullOrEmpty(permission) && userPermissions.Contains(permission))
                        {
                            row.Cells["HasPermission"].Value = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load permissions for user {username}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingPermissions = false; // Reset flag to allow event firing after loading is complete
            }
        }

        private async Task UpdateUserPermission(string username, string permission, bool grantPermission)
        {
            try
            {
                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(username);
                DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                var userPermissions = snapshot.GetValue<List<string>>("Permissions") ?? new List<string>();

                if (grantPermission)
                {
                    if (!userPermissions.Contains(permission))
                    {
                        userPermissions.Add(permission);
                    }
                }
                else
                {
                    if (userPermissions.Contains(permission))
                    {
                        userPermissions.Remove(permission);
                    }
                }

                await userDoc.UpdateAsync("Permissions", userPermissions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update permissions for user {username}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateUserStore(string username, string storeName, bool addStore)
        {
            try
            {
                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(username);
                DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                var userStores = snapshot.GetValue<List<string>>("Stores") ?? new List<string>();

                if (addStore)
                {
                    if (!userStores.Contains(storeName))
                    {
                        userStores.Add(storeName);
                    }
                }
                else
                {
                    if (userStores.Contains(storeName))
                    {
                        userStores.Remove(storeName);
                    }
                }

                await userDoc.UpdateAsync("Stores", userStores);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update stores for user {username}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm(firestoreDb);
            addUserForm.ShowDialog();

            // Reload the users list after a new user is added
            listBoxUsers.Items.Clear();
            LoadUsers();
        }

        private async void btnUserEdit_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedItem != null)
            {
                string selectedUser = listBoxUsers.SelectedItem.ToString();

                // Retrieve the user's rank from Firebase
                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(selectedUser);
                DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    string rank = snapshot.GetValue<string>("Rank") ?? string.Empty;

                    // Open AddUserForm in edit mode
                    AddUserForm editUserForm = new AddUserForm(firestoreDb, selectedUser, rank);
                    editUserForm.ShowDialog();

                    // Optionally, refresh the user list or other UI elements
                    listBoxUsers.Items.Clear();
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnUserRemove_Click(object sender, EventArgs e)
        {
            string selectedUser = listBoxUsers.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedUser))
            {
                MessageBox.Show("Please select a user to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirmationResult = MessageBox.Show(
                $"Are you sure you want to delete the user '{selectedUser}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmationResult == DialogResult.Yes)
            {
                try
                {
                    DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(selectedUser);
                    await userDoc.DeleteAsync();

                    MessageBox.Show($"User '{selectedUser}' has been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the list of users
                    listBoxUsers.Items.Remove(selectedUser);
                    ClearUserDetails();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearUserDetails()
        {

            // Also clear any selection or related data in grids
            foreach (DataGridViewRow row in dataGridViewPermissions.Rows)
            {
                row.Cells["HasPermission"].Value = false;
            }

            foreach (DataGridViewRow row in dataGridViewStoresFranchises.Rows)
            {
                row.Cells["SelectUKStore"].Value = false;
                row.Cells["SelectFranchiseStore"].Value = false;
                row.Cells["SelectFranchise"].Value = false;
            }
        }

        #endregion

        

        private void BtnExecute_Click(object sender, EventArgs e)
        {           

            string query = txtQuery1.Text;

            // Validate the query
            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("The query cannot be empty. Please enter a valid SQL query.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        connection.Open(); // Open the connection
                        adapter.Fill(dataTable); // Fill the DataTable with query results

                        if (dataTable.Rows.Count > 0)
                        {
                            // Check if the popup form is already open
                            if (popupForm == null || popupForm.IsDisposed)
                            {
                                // Create and show a new popup form
                                popupForm = new PopupForm();
                                popupForm.SetData(dataTable); // Pass the DataTable to the popup form
                                popupForm.Show();
                            }
                            else
                            {
                                // Update the existing popup form's data
                                popupForm.SetData(dataTable);
                                popupForm.BringToFront(); // Bring the popup to the front
                            }
                        }
                        else
                        {
                            MessageBox.Show("Query executed successfully, but no data was returned.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve the connection string from Web.config/App.config
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            // SQL to find the highest Client_Version from TBPC table
            string highestVersionSql = @"
            ;WITH VersionParts AS (
        SELECT 
            Client_Version,
            TRY_CAST(PARSENAME(Client_Version, 3) AS INT) AS Major,
            TRY_CAST(PARSENAME(Client_Version, 2) AS INT) AS Minor,
            TRY_CAST(PARSENAME(Client_Version, 1) AS INT) AS Patch
        FROM TBPC
    )
    SELECT TOP 1 Client_Version
    FROM VersionParts
    ORDER BY Major DESC, Minor DESC, Patch DESC;";

            // SQL to count how many machines are not on the highest version
            string countMachinesSql = @"
        SELECT COUNT(*) 
        FROM Computers AS c
        INNER JOIN TBPC AS t
            ON c.MachineName = t.Name
        WHERE t.Client_Version <> @HighestVersion";

            // SQL to update machines not on the highest version
            string updateSql = @"
        UPDATE c
        SET c.AppUpdate = 'Yes'
        FROM Computers AS c
        INNER JOIN TBPC AS t
            ON c.MachineName = t.Name
        WHERE t.Client_Version <> @HighestVersion";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Get the highest version
                    string highestVersion;
                    using (SqlCommand cmd = new SqlCommand(highestVersionSql, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            highestVersion = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Could not determine the highest version.");
                            return;
                        }
                    }

                    // Step 2: Count machines not on the highest version
                    int countNotOnLatest;
                    using (SqlCommand cmd = new SqlCommand(countMachinesSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@HighestVersion", highestVersion);
                        object result = cmd.ExecuteScalar();
                        countNotOnLatest = result != null ? Convert.ToInt32(result) : 0;
                    }

                    // Step 3: Inform the user and ask for confirmation
                    string message = $"The latest version is {highestVersion}.\n" +
                                     $"{countNotOnLatest} machine(s) are not on this version.\n" +
                                     $"Do you want to set AppUpdate to 'Yes' for these machines?";
                    DialogResult dialogResult = MessageBox.Show(message, "Confirm Update", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Step 4: Perform the update if confirmed
                        using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@HighestVersion", highestVersion);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            MessageBox.Show($"Update executed. Rows affected: {rowsAffected}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Operation cancelled by the user.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Ideally log the exception or show an error message
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Retrieve the connection string from Web.config/App.config
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            // Define your SQL update command
            string query = @"
            SELECT 
            Name,
            Store,
            Commsversion,
            TillVersion

            FROM 
            TBPC

            WHERE 
            Name LIKE '%ABL%'
        ";

            // Execute the update statement
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        connection.Open(); // Open the connection
                        adapter.Fill(dataTable); // Fill the DataTable with query results

                        if (dataTable.Rows.Count > 0)
                        {
                            // Check if the popup form is already open
                            if (popupForm == null || popupForm.IsDisposed)
                            {
                                // Create and show a new popup form
                                popupForm = new PopupForm();
                                popupForm.SetData(dataTable); // Pass the DataTable to the popup form
                                popupForm.Show();
                            }
                            else
                            {
                                // Update the existing popup form's data
                                popupForm.SetData(dataTable);
                                popupForm.BringToFront(); // Bring the popup to the front
                            }
                        }
                        else
                        {
                            MessageBox.Show("Query executed successfully, but no data was returned.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Retrieve the connection string from your config
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            // 2. Define the SQL update command
            string query = "UPDATE Computers SET TillUpdater = 'Yes' WHERE MachineName LIKE '%ABL%'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // 3. Execute the update statement
                        int rowsAffected = command.ExecuteNonQuery();

                        // 4. Inform the user how many rows were updated
                        MessageBox.Show($"Update successful! Rows affected: {rowsAffected}",
                            "Update",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ideally, log the exception. Showing as a message for demonstration.
                MessageBox.Show($"Error executing update: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}