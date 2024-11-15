using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class EmailManager : Form
    {
        private Outlook.Application outlookApp;
        private Outlook.NameSpace outlookNamespace;
        private Outlook.Store selectedStore;

        // ManualResetEventSlim for pause/resume functionality
        private ManualResetEventSlim pauseEvent;

        public EmailManager()
        {
            InitializeComponent();
            SetupDataGridViewColumns();
            LoadMailboxes();

            // Ensure event handlers are connected
            cmbMailboxes.SelectedIndexChanged += cmbMailboxes_SelectedIndexChanged;
            btnCopyEmails.Click += btnCopyEmails_Click;
            btnCleanFilenames.Click += btnCleanFilenames_Click;
            btnPauseResume.Click += btnPauseResume_Click;

            // Initialize the pauseEvent (initially not paused)
            pauseEvent = new ManualResetEventSlim(true);
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
            catch (Exception ex)
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
            catch (Exception ex)
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

            // Check if the full folder path starts with the mailboxPrefix
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
            // Disable the Copy Emails button to prevent multiple clicks
            btnCopyEmails.Enabled = false;

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string outputFolderPath = fbd.SelectedPath;

                    // Clear the progress textbox
                    txtProgress.Clear();

                    // Collect all selected folders into a list
                    List<(string entryID, string displayFolderPath)> selectedFolders = new List<(string, string)>();

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
                            selectedFolders.Add((entryID, displayFolderPath));
                        }
                    }

                    if (selectedFolders.Count == 0)
                    {
                        AppendProgressText("No folders selected. Operation canceled.");
                        MessageBox.Show("No folders selected.");
                        btnCopyEmails.Enabled = true;
                        return;
                    }

                    // Process all selected folders in a single Task.Run
                    await Task.Run(() =>
                    {
                        foreach (var folderInfo in selectedFolders)
                        {
                            // Update progress
                            AppendProgressText($"Processing folder: {folderInfo.displayFolderPath}");

                            // Copy emails from the folder
                            CopyEmailsFromFolder(folderInfo.entryID, outputFolderPath, folderInfo.displayFolderPath);
                        }
                    });

                    AppendProgressText("Emails copied successfully.");
                    MessageBox.Show("Emails copied successfully.");
                }
                else
                {
                    AppendProgressText("Operation canceled by user.");
                }
            }

            // Re-enable the Copy Emails button
            btnCopyEmails.Enabled = true;
        }

        private void btnCleanFilenames_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string targetFolderPath = fbd.SelectedPath;

                    // Clear the progress textbox
                    txtProgress.Clear();

                    AppendProgressText($"Cleaning filenames in folder: {targetFolderPath}");

                    // Clean filenames
                    CleanFilenamesInFolder(targetFolderPath);

                    AppendProgressText("Filename cleaning completed.");
                    MessageBox.Show("Filename cleaning completed.");
                }
                else
                {
                    AppendProgressText("Filename cleaning operation canceled by user.");
                }
            }
        }

        private void CleanFilenamesInFolder(string folderPath)
        {
            try
            {
                // List all files in the folder
                string[] allFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);

                if (allFiles.Length == 0)
                {
                    AppendProgressText("No files found in the specified folder.");
                    return;
                }

                AppendProgressText($"Total files found: {allFiles.Length}");

                foreach (string filePath in allFiles)
                {
                    string fileName = Path.GetFileName(filePath);
                    AppendProgressText($"Found file: {fileName}");
                }

                // Proceed to filter .msg files
                string[] msgFiles = Directory.GetFiles(folderPath, "*.msg", SearchOption.TopDirectoryOnly);

                if (msgFiles.Length == 0)
                {
                    AppendProgressText("No .msg files found in the specified folder.");
                    return;
                }

                foreach (string filePath in msgFiles)
                {
                    try
                    {
                        string fileName = Path.GetFileName(filePath);
                        string newFileName = RemoveInternetMessageIDFromFilename(fileName);

                        // Log the original and new filenames
                        AppendProgressText($"Original Filename: {fileName}");
                        AppendProgressText($"Proposed New Filename: {newFileName}");

                        if (string.IsNullOrEmpty(newFileName) || newFileName == fileName)
                        {
                            // No change needed
                            AppendProgressText("No changes required for this file.\n");
                            continue;
                        }

                        // Remove read-only attribute if set
                        FileInfo fi = new FileInfo(filePath);
                        if (fi.IsReadOnly)
                        {
                            fi.IsReadOnly = false;
                            AppendProgressText($"Removed read-only attribute from: {fileName}");
                        }

                        // Ensure the new file name is unique
                        string newFilePath = GetUniqueFileName(Path.Combine(folderPath, newFileName));

                        // Rename the file
                        File.Move(filePath, newFilePath);

                        AppendProgressText($"Renamed: {fileName} -> {Path.GetFileName(newFilePath)}\n");
                    }
                    catch (Exception ex)
                    {
                        AppendProgressText($"(Error) Failed to rename file {Path.GetFileName(filePath)}: {ex.Message}\n");
                    }
                }

                // Optionally process subfolders
                // string[] subfolders = Directory.GetDirectories(folderPath);
                // foreach (string subfolder in subfolders)
                // {
                //     CleanFilenamesInFolder(subfolder);
                // }
            }
            catch (Exception ex)
            {
                AppendProgressText($"(Error) Failed to clean filenames in folder {folderPath}: {ex.Message}\n");
            }
        }

        private string RemoveInternetMessageIDFromFilename(string fileName)
        {
            try
            {
                // Assuming the shortHash is at the beginning followed by a space
                // Example: ABCD1234 01-01-2024 sender@example.com Test Email.msg
                // Desired: 01-01-2024 sender@example.com Test Email.msg

                // Split the filename by the first space
                int firstSpaceIndex = fileName.IndexOf(' ');
                if (firstSpaceIndex <= 0)
                {
                    // Filename doesn't match the expected format; return as is
                    return fileName;
                }

                // Extract the portion after the first space
                string newFileName = fileName.Substring(firstSpaceIndex + 1);

                return newFileName;
            }
            catch (Exception ex)
            {
                // Log the exception
                AppendProgressText($"(Error) Exception in RemoveInternetMessageIDFromFilename for file {fileName}: {ex.Message}");
                // In case of any error, return the original file name
                return fileName;
            }
        }

        private void CopyEmailsFromFolder(string entryID, string outputFolderPath, string displayFolderPath)
        {
            Outlook.MAPIFolder folder = null;
            Outlook.Items items = null;

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

                    items = folder.Items;
                    Outlook.MailItem mailItem = null;

                    object item = items.GetFirst();

                    while (item != null)
                    {
                        // Check for pause signal
                        pauseEvent.Wait();

                        object nextItem = null;

                        try
                        {
                            if (item is Outlook.MailItem)
                            {
                                mailItem = item as Outlook.MailItem;
                                emailCount++;

                                string receivedDate = "";
                                string senderEmail = "";
                                string subject = "";

                                try
                                {
                                    // Get the received date and format it as DD-MM-YYYY
                                    receivedDate = mailItem.ReceivedTime.ToString("dd-MM-yyyy");

                                    // Get the sender's email address
                                    senderEmail = GetSenderEmailAddress(mailItem);

                                    // Get the subject
                                    subject = string.IsNullOrEmpty(mailItem.Subject) ? "No Subject" : mailItem.Subject;

                                    // Get the InternetMessageID
                                    string internetMessageID = GetInternetMessageID(mailItem);

                                    // Handle emails without InternetMessageID
                                    if (string.IsNullOrEmpty(internetMessageID))
                                    {
                                        // Generate a unique identifier using a hash of the email properties
                                        string uniqueString = mailItem.ReceivedTime.ToString("O") + mailItem.SenderEmailAddress + mailItem.Subject;
                                        internetMessageID = GenerateHash(uniqueString);
                                    }

                                    // Generate a hash from the EntryID to ensure uniqueness
                                    string entryIDHash = GenerateHash(mailItem.EntryID);

                                    // Shorten the hash to the first 8 characters
                                    string shortHash = entryIDHash.Substring(0, 8);

                                    // Sanitize all parts of the file name
                                    string sanitizedDate = CleanFileName(receivedDate);
                                    string sanitizedSenderEmail = CleanFileName(senderEmail);
                                    string sanitizedSubject = CleanFileName(subject);

                                    // Truncate the sender email and subject to prevent long file names
                                    if (sanitizedSenderEmail.Length > 50)
                                        sanitizedSenderEmail = sanitizedSenderEmail.Substring(0, 50);

                                    if (sanitizedSubject.Length > 50)
                                        sanitizedSubject = sanitizedSubject.Substring(0, 50);

                                    // Construct the file name with a shorter identifier
                                    string fileName = $"{shortHash} {sanitizedDate} {sanitizedSenderEmail} {sanitizedSubject}.msg";

                                    // Log the unsanitized filename
                                    AppendProgressText($"Original Filename: {fileName}");

                                    // Sanitize the entire filename again to ensure no residual invalid characters
                                    fileName = CleanFileName(fileName);

                                    // Log the sanitized filename
                                    AppendProgressText($"Sanitized Filename: {fileName}");

                                    // Limit the total filename length to prevent exceeding path length limits
                                    if (fileName.Length > 200)
                                        fileName = fileName.Substring(0, 200 - ".msg".Length) + ".msg";

                                    // Combine with the output folder path
                                    string outputFileName = Path.Combine(folderOutputPath, fileName);

                                    // Log the output filename
                                    AppendProgressText($"Attempting to save Email {emailCount} to: {outputFileName}");

                                    // Ensure the file name is unique
                                    outputFileName = GetUniqueFileName(outputFileName);

                                    // Save email to disk
                                    try
                                    {
                                        mailItem.SaveAs(outputFileName, Outlook.OlSaveAsType.olMSG);
                                        // Update progress with email number and file name
                                        AppendProgressText($"Email {emailCount}: Saved as {Path.GetFileName(outputFileName)}\n");
                                    }
                                    catch (Exception ex)
                                    {
                                        AppendProgressText($"(Error) Failed to save email {emailCount}: Date: {receivedDate}, Sender: {senderEmail}, Subject: {subject}\nError: {ex.Message}\n");
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Include date, sender, and subject in error message
                                    AppendProgressText($"(Error) Email {emailCount}: Date: {receivedDate}, Sender: {senderEmail}, Subject: {subject}\nError: {ex.Message}\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Include date, sender, and subject in error message
                            AppendProgressText($"(Error) Email {emailCount}: Error accessing mail item properties.\nError: {ex.Message}\n");
                        }
                        finally
                        {
                            // Get the next item before releasing the current item
                            try
                            {
                                nextItem = items.GetNext();
                            }
                            catch (Exception ex)
                            {
                                AppendProgressText($"(Error) Failed to get next item: {ex.Message}\n");
                                nextItem = null;
                            }

                            if (mailItem != null)
                            {
                                Marshal.ReleaseComObject(mailItem);
                                mailItem = null;
                            }

                            if (item != null)
                            {
                                Marshal.ReleaseComObject(item);
                                item = null;
                            }

                            item = nextItem;
                        }
                    }

                    // Update progress after processing the folder
                    AppendProgressText($"Copied {emailCount} emails from folder: {displayFolderPath}\n");
                }
                else
                {
                    AppendProgressText($"Folder not found with EntryID: {entryID}\n");
                }
            }
            catch (Exception ex)
            {
                AppendProgressText($"Error copying emails from folder {displayFolderPath}: {ex.Message}\n");
            }
            finally
            {
                if (items != null)
                {
                    Marshal.ReleaseComObject(items);
                    items = null;
                }
                if (folder != null)
                {
                    Marshal.ReleaseComObject(folder);
                    folder = null;
                }
            }
        }

        private string GetInternetMessageID(Outlook.MailItem mailItem)
        {
            string internetMessageID = null;
            Outlook.PropertyAccessor propertyAccessor = null;
            try
            {
                const string PR_INTERNET_MESSAGE_ID = "http://schemas.microsoft.com/mapi/proptag/0x1035001F";
                propertyAccessor = mailItem.PropertyAccessor;
                internetMessageID = propertyAccessor.GetProperty(PR_INTERNET_MESSAGE_ID) as string;
            }
            catch (Exception ex)
            {
                AppendProgressText($"(Error) Failed to get Internet Message ID: {ex.Message}\n");
                internetMessageID = null;
            }
            finally
            {
                if (propertyAccessor != null)
                {
                    Marshal.ReleaseComObject(propertyAccessor);
                    propertyAccessor = null;
                }
            }
            return internetMessageID;
        }

        private string GetSenderEmailAddress(Outlook.MailItem mailItem)
        {
            string senderEmail = string.Empty;
            Outlook.AddressEntry sender = null;
            Outlook.ExchangeUser exchUser = null;
            Outlook.ContactItem contactItem = null;

            try
            {
                sender = mailItem.Sender;
                if (sender != null)
                {
                    // If the sender is an Exchange user
                    if (sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry
                        || sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
                    {
                        exchUser = sender.GetExchangeUser();
                        if (exchUser != null)
                        {
                            senderEmail = exchUser.PrimarySmtpAddress;
                        }
                        else
                        {
                            senderEmail = sender.Name;
                        }
                    }
                    // If the sender is a contact
                    else if (sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olOutlookContactAddressEntry)
                    {
                        contactItem = sender.GetContact();
                        if (contactItem != null)
                        {
                            senderEmail = contactItem.Email1Address;
                        }
                        else
                        {
                            senderEmail = sender.Name;
                        }
                    }
                    else
                    {
                        senderEmail = sender.Address;
                    }
                }
                else
                {
                    senderEmail = mailItem.SenderName;
                }
            }
            catch
            {
                senderEmail = mailItem.SenderName;
            }
            finally
            {
                if (contactItem != null)
                {
                    Marshal.ReleaseComObject(contactItem);
                    contactItem = null;
                }
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

            // If senderEmail is still in Exchange format or empty, use the sender's name
            if (string.IsNullOrEmpty(senderEmail) || senderEmail.StartsWith("/"))
            {
                senderEmail = mailItem.SenderName;
            }

            return senderEmail;
        }

        private string CleanFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "File name cannot be null or empty.");

            // Separate the filename and extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            // Remove invalid characters from the filename part
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + @"<>:""/\|?*";

            foreach (char c in invalidChars)
            {
                fileNameWithoutExtension = fileNameWithoutExtension.Replace(c.ToString(), "");
            }

            // Additional characters to remove
            string[] additionalInvalidStrings = { "/", "\\", ":", "*", "?", "\"", "<", ">", "|", "\0", "(", ")", "[", "]", "{", "}", "@", "#", "%", "&", "$", "!", "'", "`", "~" };
            foreach (string invalidStr in additionalInvalidStrings)
            {
                fileNameWithoutExtension = fileNameWithoutExtension.Replace(invalidStr, "");
            }

            // Trim the filename to a reasonable length to avoid exceeding path length limits
            int maxLength = 200 - extension.Length; // Adjust as necessary
            if (fileNameWithoutExtension.Length > maxLength)
                fileNameWithoutExtension = fileNameWithoutExtension.Substring(0, maxLength);

            // Reconstruct the filename with the extension
            return $"{fileNameWithoutExtension}{extension}";
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

        private string GenerateHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private void AppendProgressText(string text)
        {
            if (txtProgress.InvokeRequired)
            {
                txtProgress.Invoke(new Action(() =>
                {
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

            // Dispose the pauseEvent
            if (pauseEvent != null)
            {
                pauseEvent.Dispose();
                pauseEvent = null;
            }
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            if (pauseEvent.IsSet)
            {
                // Pause the operation
                pauseEvent.Reset();
                btnPauseResume.Text = "Resume";
                AppendProgressText("Operation paused.");
            }
            else
            {
                // Resume the operation
                pauseEvent.Set();
                btnPauseResume.Text = "Pause";
                AppendProgressText("Operation resumed.");
            }
        }
    }
}
