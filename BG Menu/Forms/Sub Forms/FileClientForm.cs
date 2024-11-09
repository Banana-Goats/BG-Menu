using BG_Menu.Class.Design;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO.Compression;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class FileClientForm : Form
    {
        private const int port = 50547;
        private const string serverIp = "http://bananagoats.co.uk";
        private readonly string baseUrl = $"{serverIp}:{port}/";

        // Store the current folder path to handle navigation
        private string currentFolderPath = "";

        // Cache to store the directory structure
        private List<FolderItem> folderCache;

        private ImageList imageList;

        // Use a single instance of HttpClient
        private static readonly HttpClient client = new HttpClient();

        public FileClientForm()
        {
            InitializeComponent();
            SetupListView();
        }

        private void SetupListView()
        {
            // Initialize the ListView to behave like a file explorer
            listViewFiles.View = View.Details;
            listViewFiles.FullRowSelect = true;
            listViewFiles.Columns.Add("Name", 300);
            listViewFiles.Columns.Add("Type", 100);

            imageList = new ImageList();
            imageList.Images.Add("folder", SystemIcons.WinLogo); // Replace with your folder icon
            imageList.Images.Add("file", SystemIcons.Application); // Replace with your file icon
            listViewFiles.LargeImageList = imageList;
            listViewFiles.SmallImageList = imageList;

            // Handle double-click event for navigation
            listViewFiles.MouseDoubleClick += ListViewFiles_MouseDoubleClick;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            await ConnectToServerAsync();
        }

        private async Task ConnectToServerAsync()
        {
            try
            {
                string requestUrl = baseUrl + "listAll";
                AppendLog($"Attempting to connect to: BananaGoats Server");

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Server returns the entire folder structure as JSON
                string folderStructure = await response.Content.ReadAsStringAsync();
                folderCache = JsonConvert.DeserializeObject<List<FolderItem>>(folderStructure); // Cache the result

                if (folderCache == null || folderCache.Count == 0)
                {
                    AppendLog("No folders or files found.");
                    return;
                }

                // Clear the current folder and show the root directory
                currentFolderPath = "";
                PopulateListView(folderCache, currentFolderPath);
            }
            catch (Exception ex)
            {
                AppendLog($"Error connecting to the server: {ex.Message}");
            }
        }

        // Populate the ListView with files and folders
        private void PopulateListView(List<FolderItem> filesAndDirs, string currentFolder)
        {
            if (filesAndDirs == null)
            {
                AppendLog("Error: folderCache is null.");
                return;
            }

            // Clear the ListView before populating new items
            listViewFiles.BeginUpdate(); // Stop ListView from redrawing during updates
            listViewFiles.Items.Clear();

            // Add folders and files to the ListView that are directly in the current folder
            foreach (var item in filesAndDirs)
            {
                // Check if the item belongs to the current folder
                if (IsDirectChild(item.Path, currentFolder))
                {
                    var listItem = new ListViewItem(Path.GetFileName(item.Path))
                    {
                        Tag = item // Store the item for later use (e.g., for double-click navigation)
                    };

                    // Assign folder and file icons
                    if (item.IsDirectory)
                    {
                        listItem.ImageKey = "folder"; // Use folder icon for directories
                        listItem.SubItems.Add("Folder");
                    }
                    else
                    {
                        listItem.ImageKey = "file"; // Use file icon for files
                        listItem.SubItems.Add("File");
                    }

                    // Add the ListViewItem to the ListView control
                    listViewFiles.Items.Add(listItem);
                }
            }

            // Force redraw
            listViewFiles.EndUpdate(); // Resume ListView redraw
            listViewFiles.Refresh(); // Ensure the ListView is displayed properly
        }

        private bool IsDirectChild(string fullPath, string currentFolder)
        {
            // Normalize paths
            fullPath = fullPath.Replace('/', Path.DirectorySeparatorChar);
            currentFolder = currentFolder.Replace('/', Path.DirectorySeparatorChar);

            // If the currentFolder is empty (root), check that the item is not in a subdirectory
            if (string.IsNullOrEmpty(currentFolder.Trim()))
            {
                // The item should not contain any directory separator if it's in the root
                return fullPath.Count(c => c == Path.DirectorySeparatorChar) == 0;
            }

            // Normalize folder paths to ensure they have the same format
            currentFolder = currentFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

            // Check if the fullPath starts with the currentFolder path
            if (!fullPath.StartsWith(currentFolder, StringComparison.OrdinalIgnoreCase))
            {
                return false; // Not a direct child if fullPath doesn't start with currentFolder
            }

            // Calculate the relative path from the current folder to the full path
            string relativePath = fullPath.Substring(currentFolder.Length);

            // If the relative path does not contain a directory separator, it's a direct child
            return !relativePath.Contains(Path.DirectorySeparatorChar);
        }

        // Handle double-click event to navigate into folders
        private void ListViewFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 1)
            {
                var selectedItem = listViewFiles.SelectedItems[0];
                var folderItem = (FolderItem)selectedItem.Tag;

                if (folderItem.IsDirectory)
                {
                    // Navigate into the folder
                    currentFolderPath = folderItem.Path;
                    PopulateListView(folderCache, currentFolderPath);
                }
            }
        }

        // Go back to the parent directory
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFolderPath))
            {
                // Get the parent folder and make sure it navigates one level up
                currentFolderPath = Path.GetDirectoryName(currentFolderPath) ?? "";

                // If the currentFolderPath is now null or empty, we are back at the root
                if (string.IsNullOrEmpty(currentFolderPath))
                {
                    currentFolderPath = ""; // Reset to root
                }

                PopulateListView(folderCache, currentFolderPath);
            }
        }

        // Helper method to log messages to the RichTextBox
        private void AppendLog(string message)
        {
            if (richTextBoxLogs.InvokeRequired)
            {
                richTextBoxLogs.Invoke(new Action(() => richTextBoxLogs.AppendText($"{DateTime.Now}: {message}\n")));
            }
            else
            {
                richTextBoxLogs.AppendText($"{DateTime.Now}: {message}\n");
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 1)
            {
                var selectedItem = listViewFiles.SelectedItems[0];
                var folderItem = (FolderItem)selectedItem.Tag;

                // Construct the full URL for the file/folder on the server
                string downloadUrl = $"{baseUrl}{Uri.EscapeDataString(folderItem.Path.Replace(Path.DirectorySeparatorChar, '/'))}";

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // If the selected item is a folder, offer to save it as a ZIP file
                    if (folderItem.IsDirectory)
                    {
                        saveFileDialog.FileName = Path.GetFileName(folderItem.Path) + ".zip"; // Default ZIP file name
                        saveFileDialog.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
                    }
                    else
                    {
                        saveFileDialog.FileName = Path.GetFileName(folderItem.Path); // Default file name
                        saveFileDialog.Filter = "All files (*.*)|*.*";
                    }

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string savePath = saveFileDialog.FileName;

                        try
                        {
                            AppendLog($"Downloading from: {downloadUrl} to: {savePath}");

                            using (var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                            {
                                response.EnsureSuccessStatusCode();

                                using (var stream = await response.Content.ReadAsStreamAsync())
                                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    await stream.CopyToAsync(fileStream);
                                }
                            }

                            if (folderItem.IsDirectory)
                            {
                                string extractPath = Path.Combine(Path.GetDirectoryName(savePath), Path.GetFileNameWithoutExtension(savePath));

                                // Extract the downloaded ZIP
                                ZipFile.ExtractToDirectory(savePath, extractPath);

                                // Clean up the downloaded ZIP after extraction
                                File.Delete(savePath);
                                AppendLog($"Folder downloaded and extracted to: {extractPath}");
                            }
                            else
                            {
                                AppendLog($"File downloaded successfully to: {savePath}");
                            }

                            MessageBox.Show("Download completed successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            AppendLog($"Error downloading: {ex.Message}");
                            MessageBox.Show($"Error downloading: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a file or folder to download.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Class to represent a folder or file item
        public class FolderItem
        {
            public string Path { get; set; }
            public bool IsDirectory { get; set; }
        }

        private async void btnUploadFile_Click(object sender, EventArgs e)
        {
            await UploadFileAsync();
        }

        private async Task UploadFileAsync()
        {
            // Ensure that we are trying to upload to the currentFolderPath
            string targetFolderPath = currentFolderPath;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.Multiselect = false; // Allow uploading one file at a time
                openFileDialog.Title = "Select a file to upload";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // Escape the selected folder path and pass it as a query parameter for the upload URL
                    string uploadUrl = $"{baseUrl}upload?targetFolder={Uri.EscapeDataString(targetFolderPath.Replace(Path.DirectorySeparatorChar, '/'))}";

                    try
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            var fileStream = new FileStream(selectedFilePath, FileMode.Open, FileAccess.Read);
                            var streamContent = new StreamContent(fileStream);

                            streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "file",
                                FileName = Path.GetFileName(selectedFilePath)
                            };

                            content.Add(streamContent);

                            // Send the file to the server
                            HttpResponseMessage response = await client.PostAsync(uploadUrl, content);
                            response.EnsureSuccessStatusCode();
                            string serverResponse = await response.Content.ReadAsStringAsync();

                            AppendLog("File uploaded successfully.");
                            MessageBox.Show("File uploaded successfully!", "Upload Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            await RefreshCurrentFolderView();
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"Error uploading file: {ex.Message}");
                        MessageBox.Show($"Error uploading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnCreateFolder_Click(object sender, EventArgs e)
        {
            await CreateFolderAsync();
        }

        private async Task CreateFolderAsync()
        {
            // Ensure we are currently inside a valid folder
            string targetFolderPath = currentFolderPath;

            // Prompt the user for a new folder name
            string newFolderName = Microsoft.VisualBasic.Interaction.InputBox("Enter name for the new folder:", "Create New Folder", "New Folder");

            // Ensure the user entered a folder name
            if (string.IsNullOrWhiteSpace(newFolderName))
            {
                MessageBox.Show("Folder name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Escape the selected folder path and new folder name to pass as query parameters
            string createFolderUrl = $"{baseUrl}createFolder?currentFolderPath={Uri.EscapeDataString(targetFolderPath.Replace(Path.DirectorySeparatorChar, '/'))}&newFolderName={Uri.EscapeDataString(newFolderName)}";

            try
            {
                HttpResponseMessage response = await client.PostAsync(createFolderUrl, null); // No need for content in this case
                response.EnsureSuccessStatusCode();

                AppendLog($"Created new folder: {newFolderName}");

                await RefreshCurrentFolderView();
            }
            catch (Exception ex)
            {
                AppendLog($"Error creating folder: {ex.Message}");
                MessageBox.Show($"Error creating folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUploadFolder_Click(object sender, EventArgs e)
        {
            await UploadFolderAsync();
        }

        private async Task UploadFolderAsync()
        {
            // Ensure that we have navigated to a folder where the upload will take place
            string targetFolderPath = currentFolderPath;

            // Prompt the user to select a folder to upload
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderToUpload = folderDialog.SelectedPath;

                    // Zip the folder to prepare it for upload
                    string zipFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileName(folderToUpload)}.zip");

                    try
                    {
                        // Create the ZIP file
                        ZipFile.CreateFromDirectory(folderToUpload, zipFilePath, CompressionLevel.Fastest, false);

                        // Upload the ZIP file to the current folder path
                        string uploadUrl = $"{baseUrl}upload?targetFolder={Uri.EscapeDataString(targetFolderPath.Replace(Path.DirectorySeparatorChar, '/'))}";

                        using (var content = new MultipartFormDataContent())
                        {
                            var fileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
                            var streamContent = new StreamContent(fileStream);

                            streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "file",
                                FileName = Path.GetFileName(zipFilePath)
                            };

                            content.Add(streamContent);

                            // Send the zipped folder to the server
                            HttpResponseMessage response = await client.PostAsync(uploadUrl, content);
                            response.EnsureSuccessStatusCode();
                            string serverResponse = await response.Content.ReadAsStringAsync();

                            AppendLog("Folder uploaded and extracted successfully.");
                            MessageBox.Show("Folder uploaded and extracted successfully!", "Upload Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await RefreshCurrentFolderView();
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"Error uploading folder: {ex.Message}");
                        MessageBox.Show($"Error uploading folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        // Clean up the temp zip file after uploading
                        if (File.Exists(zipFilePath))
                        {
                            File.Delete(zipFilePath);
                            AppendLog($"Temp zip file deleted: {zipFilePath}");
                        }
                    }
                }
            }
        }

        private async Task RefreshCurrentFolderView()
        {
            try
            {
                string requestUrl = $"{baseUrl}listAll";
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Re-fetch the folder structure from the server
                string folderStructure = await response.Content.ReadAsStringAsync();
                folderCache = JsonConvert.DeserializeObject<List<FolderItem>>(folderStructure);

                // Re-populate the ListView with the latest data for the current folder
                PopulateListView(folderCache, currentFolderPath);
            }
            catch (Exception ex)
            {
                AppendLog($"Error refreshing folder view: {ex.Message}");
                MessageBox.Show($"Error refreshing folder view: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
