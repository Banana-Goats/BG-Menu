using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class UserSettings : Form
    {
        private readonly string currentUsername;
        private readonly FirestoreDb firestoreDb;

        private const string baseUrl = "http://bananagoats.co.uk:50547/";
        private const string versionNumber = "Live Build";

        public UserSettings(string Username, FirestoreDb db)
        {
            InitializeComponent();
            currentUsername = Username;
            firestoreDb = db;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddUserForm editUserForm = new AddUserForm(firestoreDb, currentUsername, isEditMode: true);
            editUserForm.ShowDialog();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            await DownloadVersionFolderAsync(versionNumber);
            button1.Enabled = true;
        }

        private async Task DownloadVersionFolderAsync(string version)
        {
            try
            {
                string downloadUrl = $"{baseUrl}BG%20Menu/{Uri.EscapeDataString(version)}";
                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BG-Menu", "BG Menu");

                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }

                string tempZipFile = Path.Combine(Path.GetTempPath(), $"{version}.zip");
                string tempExtractPath = Path.Combine(Path.GetTempPath(), $"Extracted_{version}");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(downloadUrl);
                    response.EnsureSuccessStatusCode();

                    byte[] fileData = await response.Content.ReadAsByteArrayAsync();

                    File.WriteAllBytes(tempZipFile, fileData);

                    ZipFile.ExtractToDirectory(tempZipFile, tempExtractPath);

                    CopyFilesRecursively(new DirectoryInfo(tempExtractPath), new DirectoryInfo(appDataPath));

                    File.Delete(tempZipFile);
                    Directory.Delete(tempExtractPath, true);

                    string batFilePath = CreateUpdateBatFile(appDataPath);

                    RunBatFile(batFilePath);

                    MessageBox.Show($"Successfully downloaded and extracted version {version} to {appDataPath}.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading the version folder: {ex.Message}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                DirectoryInfo targetSubDir = target.CreateSubdirectory(dir.Name);
                CopyFilesRecursively(dir, targetSubDir);
            }

            foreach (FileInfo file in source.GetFiles())
            {
                string targetFilePath = Path.Combine(target.FullName, file.Name);
                file.CopyTo(targetFilePath, true);
            }
        }

        private string CreateUpdateBatFile(string appDataPath)
        {
            try
            {
                string batFilePath = Path.Combine(appDataPath, "UpdateBGMenu.bat");
                string sourcePath = appDataPath;
                string destinationPath = @"C:\BG Menu";
                string exePath = Path.Combine(destinationPath, "BG Menu.exe");

                if (File.Exists(batFilePath))
                {
                    File.Delete(batFilePath);
                }

                string batContent = $@"
                                    @echo off
                                    echo Killing the BG Menu.exe process...
                                    taskkill /f /im ""BG Menu.exe"" >nul 2>nul
                                    timeout /t 2 /nobreak >nul
                                    echo Copying files from ""{sourcePath}"" to ""{destinationPath}""...
                                    xcopy /e /y /i ""{sourcePath}"" ""{destinationPath}"" > ""%temp%\update_log.txt""
                                    echo Changing directory to ""{destinationPath}""...
                                    cd /d ""{destinationPath}""  // Change to the destination folder
                                    echo Starting the BG Menu.exe...
                                    start """" ""{exePath}""
                                    exit
                                    ";

                File.WriteAllText(batFilePath, batContent);

                return batFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating the .bat file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void RunBatFile(string batFilePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(batFilePath) && File.Exists(batFilePath))
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo()
                    {
                        FileName = batFilePath,
                        UseShellExecute = true,
                        Verb = "runas"
                    };
                    Process.Start(processInfo);
                }
                else
                {
                    MessageBox.Show("The .bat file does not exist or could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running the .bat file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
