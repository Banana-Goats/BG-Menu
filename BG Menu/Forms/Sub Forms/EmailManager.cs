using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.IO;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class EmailManager : Form
    {
        private Outlook.Application outlookApp;
        private Outlook.NameSpace outlookNamespace;
        private Outlook.Store selectedStore;

        public EmailManager()
        {
            InitializeComponent();
            SetupDataGridViewColumns();
            LoadMailboxes();

            // Ensure event handlers are connected
            cmbMailboxes.SelectedIndexChanged += cmbMailboxes_SelectedIndexChanged;
            button1.Click += btnCopyEmails_Click;
        }

        private void SetupDataGridViewColumns()
        {
            // Clear existing columns (if any)
            dgvFolders.Columns.Clear();

            // Create checkbox column
            DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
            chkColumn.Name = "Select";
            chkColumn.HeaderText = "Select";
            dgvFolders.Columns.Add(chkColumn);

            // Create text column for display folder path
            DataGridViewTextBoxColumn txtColumn = new DataGridViewTextBoxColumn();
            txtColumn.Name = "FolderPath";
            txtColumn.HeaderText = "Folder Path";
            dgvFolders.Columns.Add(txtColumn);

            // Create hidden column for EntryID
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.Name = "EntryID";
            idColumn.HeaderText = "EntryID";
            idColumn.Visible = false; // Hide this column
            dgvFolders.Columns.Add(idColumn);
        }

        private void LoadMailboxes()
        {
            try
            {
                // Initialize Outlook Application
                outlookApp = new Outlook.Application();
                outlookNamespace = outlookApp.GetNamespace("MAPI");

                // Get all mail stores (mailboxes)
                foreach (Outlook.Store store in outlookNamespace.Stores)
                {
                    cmbMailboxes.Items.Add(store.DisplayName);
                }

                if (cmbMailboxes.Items.Count > 0)
                    cmbMailboxes.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error loading mailboxes: " + ex.Message);
            }
        }

        private void cmbMailboxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFolders();
        }

        private void LoadFolders()
        {
            dgvFolders.Rows.Clear();
            try
            {
                string selectedMailbox = cmbMailboxes.SelectedItem.ToString();
                selectedStore = null;

                foreach (Outlook.Store store in outlookNamespace.Stores)
                {
                    if (store.DisplayName == selectedMailbox)
                    {
                        selectedStore = store;
                        break;
                    }
                }

                if (selectedStore != null)
                {
                    // Start from the root folder of the selected mailbox
                    Outlook.MAPIFolder rootFolder = selectedStore.GetRootFolder();

                    // Load folders recursively
                    LoadFoldersRecursive(rootFolder);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error loading folders: " + ex.Message);
            }
        }

        private void LoadFoldersRecursive(Outlook.MAPIFolder folder)
        {
            foreach (Outlook.MAPIFolder subFolder in folder.Folders)
            {
                // Get the full folder path
                string fullFolderPath = subFolder.FolderPath;

                // Remove the mailbox name from the folder path
                string displayFolderPath = GetDisplayFolderPath(fullFolderPath);

                // Add the processed folder path and EntryID to the DataGridView
                dgvFolders.Rows.Add(false, displayFolderPath, subFolder.EntryID);

                // Recursive call to get subfolders
                LoadFoldersRecursive(subFolder);

                // Release COM object
                Marshal.ReleaseComObject(subFolder);
            }
        }

        private string GetDisplayFolderPath(string fullFolderPath)
        {
            // Get the selected mailbox name
            string selectedMailbox = cmbMailboxes.SelectedItem.ToString().Trim();

            // Normalize the full folder path
            string normalizedFullPath = fullFolderPath.Trim();

            // Build the mailbox prefix to remove (e.g., "\\MailboxName")
            string mailboxPrefix = @"\" + selectedMailbox;

            // Check if the full folder path starts with the mailbox prefix
            if (normalizedFullPath.StartsWith(mailboxPrefix, StringComparison.OrdinalIgnoreCase))
            {
                // Remove the mailbox prefix from the folder path
                string pathWithoutMailbox = normalizedFullPath.Substring(mailboxPrefix.Length);

                // Remove any leading backslashes
                return pathWithoutMailbox.TrimStart('\\');
            }
            else if (normalizedFullPath.StartsWith(@"\")) // For root folders
            {
                // Remove the leading backslash
                return normalizedFullPath.TrimStart('\\');
            }
            else
            {
                // Return the folder path as is
                return normalizedFullPath;
            }
        }

        private async void btnCopyEmails_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string outputFolderPath = fbd.SelectedPath;

                    List<Task> tasks = new List<Task>();

                    foreach (DataGridViewRow row in dgvFolders.Rows)
                    {
                        if (row == null)
                            continue;

                        // Ensure the cells exist and have values
                        var selectCell = row.Cells["Select"];
                        var entryIDCell = row.Cells["EntryID"];

                        if (selectCell?.Value == null || entryIDCell?.Value == null)
                            continue;

                        bool isChecked = Convert.ToBoolean(selectCell.Value);

                        if (isChecked)
                        {
                            string entryID = entryIDCell.Value.ToString();

                            // Run the email copying in a task
                            tasks.Add(Task.Run(() => CopyEmailsFromFolder(entryID, outputFolderPath)));
                        }
                    }

                    // Await all tasks to complete
                    await Task.WhenAll(tasks);

                    MessageBox.Show("Emails copied successfully");
                }
            }
        }

        private void CopyEmailsFromFolder(string entryID, string outputFolderPath)
        {
            try
            {
                // Ensure that selectedStore is not null
                if (selectedStore == null)
                {
                    MessageBox.Show("Selected store is null.");
                    return;
                }

                // Ensure outlookNamespace is initialized
                if (outlookNamespace == null)
                {
                    outlookApp = new Outlook.Application();
                    outlookNamespace = outlookApp.GetNamespace("MAPI");
                }

                // Get the folder using EntryID and StoreID
                Outlook.MAPIFolder folder = outlookNamespace.GetFolderFromID(entryID, selectedStore.StoreID);

                if (folder != null)
                {
                    string folderOutputPath = Path.Combine(outputFolderPath, SanitizeFolderName(folder.Name));
                    Directory.CreateDirectory(folderOutputPath);

                    List<Task> saveTasks = new List<Task>();

                    foreach (object item in folder.Items)
                    {
                        if (item is Outlook.MailItem mailItem)
                        {
                            // Capture variables for closure
                            var mailItemCopy = mailItem;
                            var folderPathCopy = folderOutputPath;

                            // Save email asynchronously
                            saveTasks.Add(Task.Run(() =>
                            {
                                string subject = string.IsNullOrEmpty(mailItemCopy.Subject) ? "No Subject" : mailItemCopy.Subject;
                                string outputFileName = Path.Combine(folderPathCopy, CleanFileName(subject) + ".msg");

                                // Ensure the file name is unique to prevent overwriting
                                outputFileName = GetUniqueFileName(outputFileName);

                                // Save email to disk
                                mailItemCopy.SaveAs(outputFileName, Outlook.OlSaveAsType.olMSG);

                                // Release COM object
                                Marshal.ReleaseComObject(mailItemCopy);
                            }));
                        }
                    }

                    // Wait for all save tasks to complete
                    Task.WaitAll(saveTasks.ToArray());

                    Marshal.ReleaseComObject(folder);
                }
                else
                {
                    MessageBox.Show("Folder not found with EntryID: " + entryID);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error copying emails: " + ex.Message);
            }
        }

        private string CleanFileName(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        private string SanitizeFolderName(string folderName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                folderName = folderName.Replace(c, '_');
            }
            return folderName;
        }

        private string GetUniqueFileName(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            int count = 1;

            while (File.Exists(filePath))
            {
                filePath = Path.Combine(directory, $"{fileNameWithoutExt}({count}){extension}");
                count++;
            }

            return filePath;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (outlookNamespace != null)
            {
                Marshal.ReleaseComObject(outlookNamespace);
                outlookNamespace = null;
            }

            if (outlookApp != null)
            {
                Marshal.ReleaseComObject(outlookApp);
                outlookApp = null;
            }
        }
    }
}
