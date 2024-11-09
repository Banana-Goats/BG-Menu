using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class ActiveDirectory : Form
    {
        private List<UserPrincipal> _userList;
        private UserPrincipal _selectedUser;

        public ActiveDirectory()
        {
            InitializeComponent();
            LoadUsersFromAD();
            chkAccountLocked.CheckedChanged += ChkAccountLocked_CheckedChanged;
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
        }
    }
}
