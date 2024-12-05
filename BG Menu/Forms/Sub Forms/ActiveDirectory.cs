using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class ActiveDirectory : Form
    {
        private List<UserPrincipal> _userList;
        private UserPrincipal _selectedUser;

        private readonly string connectionString = "Server=Bananagoats.co.uk;Database=Ableworld;User Id=Elliot;Password=1234;";

        public ActiveDirectory()
        {
            InitializeComponent();
            LoadUsersFromAD();
            chkAccountLocked.CheckedChanged += ChkAccountLocked_CheckedChanged;

            // Set up DataGridView event handler for row selection
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
        }

        private void LoadUsersFromAD()
        {
            _userList = new List<UserPrincipal>();

            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    using (UserPrincipal userPrincipal = new UserPrincipal(context))
                    {
                        using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
                        {
                            foreach (var result in searcher.FindAll())
                            {
                                UserPrincipal user = result as UserPrincipal;
                                if (user != null)
                                {
                                    _userList.Add(user);
                                }
                            }
                        }
                    }
                }

                // Populate the ComboBox with all users' usernames
                cmbUsers.Items.AddRange(_userList.Select(u => u.SamAccountName).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users from Active Directory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }



        // Load CSV into DataGridView
        private void LoadCSV(string filePath)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("User");
                dataTable.Columns.Add("Password");

                string[] csvLines = File.ReadAllLines(filePath);

                foreach (string line in csvLines)
                {
                    string[] data = line.Split(',');

                    if (data.Length >= 2)
                    {
                        dataTable.Rows.Add(data[0], data[1]);
                    }
                }

                dgvUsers.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading CSV: {ex.Message}");
            }
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                // Define your SQL query
                string query = "SELECT Username, Password, Department FROM ADAccounts";

                // Create a new DataTable to hold the data
                DataTable dataTable = new DataTable();

                // Establish a connection to the SQL Server
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create a SqlDataAdapter to execute the query and fill the DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dataTable);
                    }

                    // Optionally, you can close the connection explicitly
                    conn.Close();
                }

                // Bind the DataTable to the DataGridView
                dgvUsers.DataSource = dataTable;

                // Optional: Adjust DataGridView settings for better display
                dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvUsers.ReadOnly = true;
                dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL-related exceptions
                MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handle row selection in DataGridView
        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvUsers.SelectedRows[0];

                // Populate cmbUsers and txtNewPassword with selected user and password
                string selectedUsername = selectedRow.Cells["Username"].Value.ToString();
                string selectedPassword = selectedRow.Cells["Password"].Value.ToString();

                cmbUsers.SelectedItem = selectedUsername; // This triggers cmbUsers_SelectedIndexChanged
                txtNewPassword.Text = selectedPassword;
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUsername = cmbUsers.SelectedItem?.ToString();
            if (selectedUsername != null)
            {
                _selectedUser = _userList.FirstOrDefault(u => u.SamAccountName.Equals(selectedUsername, StringComparison.OrdinalIgnoreCase));
                if (_selectedUser != null)
                {
                    txtUsername.Text = _selectedUser.SamAccountName;
                    txtEmail.Text = _selectedUser.EmailAddress;
                    chkAccountLocked.Checked = _selectedUser.IsAccountLockedOut();
                    txtDescription.Text = _selectedUser.Description ?? "No description available";
                }
            }
        }

        private void ChkAccountLocked_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedUser != null && !chkAccountLocked.Checked)
            {
                try
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                    {
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, _selectedUser.SamAccountName);
                        if (user != null && user.IsAccountLockedOut())
                        {
                            user.UnlockAccount();
                            MessageBox.Show($"The account for {user.SamAccountName} has been unlocked.", "Account Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error unlocking the account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_selectedUser != null)
            {
                string newPassword = txtNewPassword.Text;

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    MessageBox.Show("Password cannot be empty.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                    {
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, _selectedUser.SamAccountName);
                        if (user != null)
                        {
                            user.SetPassword(newPassword);
                            MessageBox.Show($"Password for {user.SamAccountName} has been changed successfully.", "Password Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    UpdatePasswordInDatabase(_selectedUser.SamAccountName, newPassword);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error changing password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user first.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            LoadDataFromDatabase();
        }

        private void UpdatePasswordInDatabase(string username, string newPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Start a transaction to ensure both operations are atomic
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Define the SQL UPDATE query with parameters to prevent SQL injection
                            string updateQuery = "UPDATE ADAccounts SET Password = @Password WHERE Username = @Username";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction))
                            {
                                // Define parameters and assign values
                                updateCmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = newPassword;
                                updateCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                                // Execute the UPDATE command
                                int rowsAffected = updateCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show($"Password for {username} has been updated successfully in the database.", "Database Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    // User does not exist, perform INSERT
                                    string insertQuery = "INSERT INTO ADAccounts (Username, Password, Department) VALUES (@Username, @Password, @Department)";

                                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction))
                                    {
                                        // Define parameters and assign values
                                        insertCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                                        insertCmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = newPassword;
                                        insertCmd.Parameters.Add("@Department", SqlDbType.VarChar, 50).Value = "Please Update"; // Default Department

                                        int insertRows = insertCmd.ExecuteNonQuery();

                                        if (insertRows > 0)
                                        {
                                            MessageBox.Show($"User {username} was not found in the database and has been added successfully.", "User Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Failed to add user {username} to the database.", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                }
                            }

                            // Commit the transaction if both operations succeed
                            transaction.Commit();

                            // Optionally, refresh the DataGridView to reflect the changes
                            LoadDataFromDatabase();
                        }
                        catch
                        {
                            // Rollback the transaction if any operation fails
                            transaction.Rollback();
                            throw;
                        }
                    }

                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL-related exceptions
                MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                MessageBox.Show($"Error updating database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadUsersFromMultipleOUs();
        }

        private void LoadUsersFromMultipleOUs()
        {
            try
            {
                // List of OUs to search in Active Directory
                List<string> ouPaths = new List<string>
                {
                    "OU=UK Stores,OU=Stores,OU=Ableworld,DC=ableworld,DC=local",
                    "OU=Franchise Stores,OU=Stores,OU=Ableworld,DC=ableworld,DC=local",
                    "OU=Franchise Services,OU=Stores,OU=Ableworld,DC=ableworld,DC=local",
                };

                List<UserEntry> allUsers = new List<UserEntry>();

                // Iterate over each OU and load users into allUsers list
                foreach (string ouPath in ouPaths)
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "ableworld.local", ouPath))
                    {
                        using (UserPrincipal userPrincipal = new UserPrincipal(context))
                        {
                            using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
                            {
                                foreach (var result in searcher.FindAll())
                                {
                                    UserPrincipal user = result as UserPrincipal;
                                    if (user != null)
                                    {
                                        allUsers.Add(new UserEntry
                                        {
                                            UserName = user.SamAccountName,
                                            Email = user.EmailAddress ?? "No Email",
                                            OU = ouPath
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                // Sort users according to the custom rules
                var sortedUsers = allUsers.OrderBy(u => GetOUIndex(u.OU))
                                          .ThenBy(u => GetUserTypePriority(u.UserName))
                                          .ThenBy(u => u.UserName)
                                          .ToList();

                // Initialize DataGridView columns if not already present
                if (ADImportdsg.Columns.Count == 0)
                {
                    ADImportdsg.Columns.Add("User", "User");
                    ADImportdsg.Columns.Add("Email", "Email");
                }

                // Clear existing rows in DataGridView to start fresh
                ADImportdsg.Rows.Clear();

                // Populate DataGridView with sorted users
                foreach (var user in sortedUsers)
                {
                    ADImportdsg.Rows.Add(user.UserName, user.Email);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users from Active Directory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to determine the order of OUs
        private int GetOUIndex(string ouPath)
        {
            if (ouPath.Contains("OU=UK Stores"))
                return 1;
            if (ouPath.Contains("OU=Franchise Stores"))
                return 2;
            if (ouPath.Contains("OU=Franchise Services"))
                return 3;

            return 4; // Default index if not found
        }

        // Helper method to determine the priority of the user type within the OU
        private int GetUserTypePriority(string userName)
        {
            if (userName.ToLower().Contains("manager"))
                return 2; // Managers should be listed second
            if (userName.ToLower().Contains("workshop"))
                return 3; // Workshops should be listed third

            return 1; // Stores should be listed first (default)
        }

        // Custom data structure to store user details along with their OU
        private class UserEntry
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string OU { get; set; }
        }

        private void BtnSendEmail_Click(object sender, EventArgs e)
        {
            // Get selected row details from dgvUsers
            if (dgvUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvUsers.SelectedRows[0];
                string selectedUsername = selectedRow.Cells["Username"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(selectedUsername))
                {
                    MessageBox.Show("The selected user does not have a valid username.", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the user's logon name from Active Directory
                string userLogonName = string.Empty;

                try
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "ableworld.local"))
                    {
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, selectedUsername);
                        if (user != null)
                        {
                            userLogonName = user.UserPrincipalName; // Get the UPN
                            if (string.IsNullOrWhiteSpace(userLogonName))
                            {
                                // Fall back to SamAccountName if UPN is not set
                                userLogonName = user.SamAccountName + "@ableworld.co.uk";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unable to find the selected user in Active Directory.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error accessing Active Directory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(userLogonName))
                {
                    MessageBox.Show("The user does not have a valid logon name.", "Invalid Logon Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedPassword = txtNewPassword.Text; // Assume password is entered here before sending

                if (string.IsNullOrWhiteSpace(selectedPassword))
                {
                    MessageBox.Show("Please enter a password before sending an email.", "No Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string todaysDate = DateTime.Now.ToString("dd/MM/yyyy");

                // Construct the email content
                string subject = "Your Email Password Will Be Changed";
                string body = $"Hello,%0D%0A%0D%0A" +
                              $"As of the {todaysDate}, your email password will be changed to the following:%0D%0A%0D%0A" +
                              $"PASSWORD: {selectedPassword}%0D%0A%0D%0A" +
                              $"You do not need to do anything at this moment in time, but please keep the password handy as you will not be able to read this message once the password changes.%0D%0A%0D%0A" +
                              $"If you have any issues, please contact IT.%0D%0A%0D%0A" +
                              $"Regards,%0D%0A%0D%0A" +
                              $"IT Support";

                // Open the default email client with the pre-populated information
                string mailtoLink = $"mailto:{userLogonName}?subject={Uri.EscapeDataString(subject)}&body={body}";
                try
                {
                    Process.Start(new ProcessStartInfo(mailtoLink) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening email client: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user first.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

