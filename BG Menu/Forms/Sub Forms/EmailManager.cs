using System;
using System.Collections.Generic;
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
            btnCopyEmails.Click += btnCopyEmails_Click;
        }

        private void SetupDataGridViewColumns()
        {
            // Clear existing columns (if any)
            dgvFolders.Columns.Clear();

            // Create checkbox column
            DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
            chkColumn.Name = "Select";
            chkColumn.HeaderText = "Select";
            chkColumn.Width = 50; // Set the desired width in pixels
            chkColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Prevent automatic resizing
            chkColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center header text
            chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center checkbox
            dgvFolders.Columns.Add(chkColumn);

            // Create text column for display folder path
            DataGridViewTextBoxColumn txtColumn = new DataGridViewTextBoxColumn();
            txtColumn.Name = "FolderPath";
            txtColumn.HeaderText = "Folder Path";
            txtColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Fill remaining space
            txtColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center header text
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
                    Marshal.ReleaseComObject(store);
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
                        // Do not release selectedStore here
                    }
                    else
                    {
                        Marshal.ReleaseComObject(store);
                    }
                }

                if (selectedStore != null)
                {
                    // Start from the root folder of the selected mailbox
                    Outlook.MAPIFolder rootFolder = selectedStore.GetRootFolder();

                    // Load folders recursively
                    LoadFoldersRecursive(rootFolder);

                    Marshal.ReleaseComObject(rootFolder);
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

                    // Clear the progress textbox
                    txtProgress.Clear();

                    foreach (DataGridViewRow row in dgvFolders.Rows)
                    {
                        if (row == null)
                            continue;

                        // Ensure the cells exist and have values
                        var selectCell = row.Cells["Select"];
                        var entryIDCell = row.Cells["EntryID"];
                        var folderPathCell = row.Cells["FolderPath"];

                        if (selectCell?.Value == null || entryIDCell?.Value == null || folderPathCell?.Value == null)
                            continue;

                        bool isChecked = Convert.ToBoolean(selectCell.Value);

                        if (isChecked)
                        {
                            string entryID = entryIDCell.Value.ToString();
                            string displayFolderPath = folderPathCell.Value.ToString();

                            // Update progress
                            AppendProgressText($"Processing folder: {displayFolderPath}");

                            // Copy emails from the folder
                            await Task.Run(() => CopyEmailsFromFolder(entryID, outputFolderPath, displayFolderPath));
                        }
                    }

                    AppendProgressText("Emails copied successfully.");
                    MessageBox.Show("Emails copied successfully.");
                }
            }
        }

        private void CopyEmailsFromFolder(string entryID, string outputFolderPath, string displayFolderPath)
        {
            Outlook.MAPIFolder folder = null;

            try
            {
                // Ensure that selectedStore is not null
                if (selectedStore == null)
                {
                    AppendProgressText("Selected store is null.");
                    return;
                }

                // Ensure outlookNamespace is initialized
                if (outlookNamespace == null)
                {
                    outlookApp = new Outlook.Application();
                    outlookNamespace = outlookApp.GetNamespace("MAPI");
                }

                // Get the folder using EntryID and StoreID
                folder = outlookNamespace.GetFolderFromID(entryID, selectedStore.StoreID);

                if (folder != null)
                {
                    // Sanitize the folder name to remove invalid characters
                    string sanitizedFolderName = SanitizeFolderName(folder.Name);

                    string folderOutputPath = Path.Combine(outputFolderPath, sanitizedFolderName);
                    Directory.CreateDirectory(folderOutputPath);

                    int emailCount = 0;
                    int totalItems = folder.Items.Count;

                    for (int i = 1; i <= totalItems; i++)
                    {
                        object itemObject = null;
                        Outlook.MailItem mailItem = null;

                        try
                        {
                            itemObject = folder.Items[i];

                            if (itemObject is Outlook.MailItem)
                            {
                                mailItem = itemObject as Outlook.MailItem;
                                emailCount++;

                                try
                                {
                                    // Get the received date and format it as DD-MM-YYYY
                                    string receivedDate = mailItem.ReceivedTime.ToString("dd-MM-yyyy");

                                    // Get the sender's email address
                                    string senderEmail = GetSenderEmailAddress(mailItem);

                                    // Get the subject
                                    string subject = string.IsNullOrEmpty(mailItem.Subject) ? "No Subject" : mailItem.Subject;

                                    // Sanitize all parts of the file name
                                    string sanitizedDate;
                                    string sanitizedSenderEmail;
                                    string sanitizedSubject;

                                    try
                                    {
                                        sanitizedDate = CleanFileName(receivedDate);
                                        sanitizedSenderEmail = CleanFileName(senderEmail);
                                        sanitizedSubject = CleanFileName(subject);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log error to txtProgress
                                        AppendProgressText($"(Error) Email {emailCount}: Error cleaning file name.\nSubject: {subject}\nError: {ex.Message}");
                                        // Continue to next email
                                        continue;
                                    }

                                    // Construct the file name
                                    string fileName = $"{sanitizedDate} {sanitizedSenderEmail} {sanitizedSubject}.msg";

                                    // Combine with the output folder path
                                    string outputFileName = Path.Combine(folderOutputPath, fileName);

                                    // Ensure the file name is unique to prevent overwriting
                                    outputFileName = GetUniqueFileName(outputFileName);

                                    // Save email to disk
                                    mailItem.SaveAs(outputFileName, Outlook.OlSaveAsType.olMSG);

                                    // Update progress with email number and file name
                                    AppendProgressText($"Email {emailCount}: {Path.GetFileName(outputFileName)}");
                                }
                                catch (Exception ex)
                                {
                                    AppendProgressText($"(Error) Email {emailCount}: {ex.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendProgressText($"(Error) Email {emailCount}: {ex.Message}");
                        }
                        finally
                        {
                            if (mailItem != null)
                            {
                                Marshal.ReleaseComObject(mailItem);
                                mailItem = null;
                            }

                            if (itemObject != null)
                            {
                                Marshal.ReleaseComObject(itemObject);
                                itemObject = null;
                            }
                        }
                    }

                    // Update progress after processing the folder
                    AppendProgressText($"Copied {emailCount} emails from folder: {displayFolderPath}");
                }
                else
                {
                    AppendProgressText($"Folder not found with EntryID: {entryID}");
                }
            }
            catch (System.Exception ex)
            {
                AppendProgressText($"Error copying emails from folder {displayFolderPath}: {ex.Message}");
            }
            finally
            {
                if (folder != null)
                {
                    Marshal.ReleaseComObject(folder);
                    folder = null;
                }
            }
        }

        private string GetSenderEmailAddress(Outlook.MailItem mailItem)
        {
            string senderEmail = "Unknown Sender";
            Outlook.AddressEntry sender = null;
            Outlook.ExchangeUser exchUser = null;

            try
            {
                if (mailItem.SenderEmailType == "SMTP")
                {
                    senderEmail = mailItem.SenderEmailAddress;
                }
                else if (mailItem.SenderEmailType == "EX")
                {
                    sender = mailItem.Sender;
                    if (sender != null)
                    {
                        exchUser = sender.GetExchangeUser();
                        if (exchUser != null)
                        {
                            senderEmail = exchUser.PrimarySmtpAddress;
                        }
                        else
                        {
                            senderEmail = sender.Address;
                        }
                    }
                }
                else
                {
                    senderEmail = mailItem.SenderEmailAddress;
                }
            }
            catch
            {
                senderEmail = "Unknown Sender";
            }
            finally
            {
                if (exchUser != null)
                {
                    Marshal.ReleaseComObject(exchUser);
                    exchUser = null;
                }
                if (sender != null)
                {
                    Marshal.ReleaseComObject(sender);
                    sender = null;
                }
            }

            return senderEmail;
        }

        private string CleanFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "File name cannot be null or empty.");

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            // Also replace any additional invalid characters
            fileName = fileName.Replace("/", "_");
            fileName = fileName.Replace("\\", "_");
            fileName = fileName.Replace(":", "_");
            fileName = fileName.Replace("*", "_");
            fileName = fileName.Replace("?", "_");
            fileName = fileName.Replace("\"", "_");
            fileName = fileName.Replace("<", "_");
            fileName = fileName.Replace(">", "_");
            fileName = fileName.Replace("|", "_");

            // Trim the file name to a reasonable length
            if (fileName.Length > 150)
                fileName = fileName.Substring(0, 150);

            return fileName;
        }

        private string SanitizeFolderName(string folderName)
        {
            return CleanFileName(folderName);
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

        private void AppendProgressText(string text)
        {
            if (txtProgress.InvokeRequired)
            {
                txtProgress.Invoke(new Action(() => {
                    txtProgress.AppendText(text + Environment.NewLine);
                }));
            }
            else
            {
                txtProgress.AppendText(text + Environment.NewLine);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (selectedStore != null)
            {
                Marshal.ReleaseComObject(selectedStore);
                selectedStore = null;
            }

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
