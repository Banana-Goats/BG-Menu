using Microsoft.Office.Core;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using PdfSharp.Charting;
using PdfSharp.Snippets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class DashBoard : Form
    {
        private List<HostData> hostList = new List<HostData>();
        private Timer pingTimer;

        public DashBoard()
        {
            InitializeComponent();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;

            LoadHosts();
            PopulateDataGridView();

            // Perform the first ping on form load
            Task.Run(async () => await PingHostsAsync());

            InitializePingTimer();
            InitializeTimer();
            LoadFolders();
            LoadFaultsAsync();
        }


        private void LoadFolders()
        {
            string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";
            var directories = Directory.GetDirectories(targetDirectory)
                                       .Where(dir => Path.GetFileName(dir).StartsWith("RecordingFiles-"))
                                       .ToList();

            dataGridView2.Rows.Clear();

            foreach (var dir in directories)
            {
                string folderName = Path.GetFileName(dir);
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView2);
                row.Cells[0].Value = folderName;

                DataGridViewButtonCell buttonCell = new DataGridViewButtonCell
                {
                    Value = "Rename"
                };
                row.Cells[1] = buttonCell;

                dataGridView2.Rows.Add(row);
            }
        }

        private async void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is the button column
            if (e.ColumnIndex == 1) // Assuming the button is in the second column
            {
                string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";
                string folderName = dataGridView2.Rows[e.RowIndex].Cells[0].Value?.ToString();

                // Validate folder name
                if (string.IsNullOrEmpty(folderName))
                {
                    MessageBox.Show("Folder name is empty. Please check the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fullPath = Path.Combine(targetDirectory, folderName);

                // Check if the folder exists
                if (!Directory.Exists(fullPath))
                {
                    MessageBox.Show($"The directory '{fullPath}' does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Generate new folder name
                string newName = GetNewFolderName(folderName);
                if (string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show("Failed to generate a new name for the folder. Check the folder format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string newFullPath = Path.Combine(targetDirectory, newName);

                // Attempt to rename the folder
                try
                {
                    Directory.Move(fullPath, newFullPath);
                    AppendProgress($"Folder renamed to: {newName}");

                    // Convert WAV files to MP3 in the renamed folder
                    await ConvertWavToMp3Async(newFullPath);

                    // Delete WAV files after conversion
                    var wavFiles = Directory.GetFiles(newFullPath, "*.wav");
                    if (wavFiles.Length > 0)
                    {
                        await DeleteWavFilesAsync(wavFiles);
                    }

                    // Rename MP3 files in the folder
                    RenameMp3Files(newFullPath);

                    // Refresh the DataGridView
                    LoadFolders();

                    // Validation option temporarily removed:
                    // DialogResult validateResult = MessageBox.Show(
                    //     "Do you want to validate the calls by importing a CSV file?",
                    //     "Validate Calls",
                    //     MessageBoxButtons.YesNo,
                    //     MessageBoxIcon.Question);

                    // if (validateResult == DialogResult.Yes)
                    // {
                    //     await ValidateCallsAsync(newFullPath, DateTime.Now);
                    // }
                }
                catch (Exception ex)
                {
                    AppendProgress($"Error during operations: {ex.Message}");
                    MessageBox.Show($"Error during operations:\nSource Path: {fullPath}\nDestination Path: {newFullPath}\nError: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetNewFolderName(string folderName)
        {
            // Extract the date part
            var parts = folderName.Split('-');
            if (parts.Length < 3) return null;

            string datePart = parts.Last();
            if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                // Format the date without invalid characters
                string formattedDate = date.ToString("dd-MM-yyyy");

                if (folderName.Contains("Daily Backup"))
                {
                    return $"{formattedDate}";
                }
                else
                {
                    string remainingPart = string.Join(" - ", parts.Skip(1).Take(parts.Length - 2));
                    return $"{remainingPart} - {formattedDate}";
                }
            }

            return null;
        }

        private async Task ConvertWavToMp3Async(string folderPath)
        {
            var wavFiles = Directory.GetFiles(folderPath, "*.wav");

            if (wavFiles.Length > 0)
            {
                AppendProgress("Converting WAV files to MP3...");
                int fileCount = 1;

                foreach (var wavFile in wavFiles)
                {
                    string mp3File = Path.ChangeExtension(wavFile, ".mp3");

                    try
                    {
                        // Update progress without showing file names
                        AppendProgress($"Converting file {fileCount}/{wavFiles.Length}...", overwrite: true);

                        using (var reader = new NAudio.Wave.AudioFileReader(wavFile))
                        using (var writer = new NAudio.Lame.LameMP3FileWriter(mp3File, reader.WaveFormat, NAudio.Lame.LAMEPreset.VBR_90))
                        {
                            await Task.Run(() => reader.CopyTo(writer));
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendProgress($"Error converting file: {Path.GetFileName(wavFile)}. Error: {ex.Message}");
                    }

                    fileCount++;
                }

                AppendProgress("All WAV files have been converted to MP3.");
            }
            else
            {
                AppendProgress("No WAV files found in the folder.");
            }
        }

        private void RenameMp3Files(string folderPath)
        {
            var mp3Files = Directory.GetFiles(folderPath, "*.mp3");

            AppendProgress("Renaming MP3 files...");
            int fileCount = 1;

            foreach (var file in mp3Files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                try
                {
                    // Update progress without showing file names
                    AppendProgress($"Renaming file {fileCount}/{mp3Files.Length}...", overwrite: true);

                    string[] parts = fileName.Split('-');
                    if (parts.Length < 5) continue;

                    string originalTimestamp = parts[0];
                    DateTime timestamp = DateTime.ParseExact(originalTimestamp, "yyyyMMddHHmmss", null);
                    string formattedTimestamp = timestamp.ToString("dd-MM-yyyy HH-mm");

                    string caller = parts[2];
                    string callee = parts[3];

                    string newFileName = $"{formattedTimestamp} ( {caller} - {callee} ).mp3";
                    string newFilePath = Path.Combine(folderPath, newFileName);

                    File.Move(file, newFilePath);
                }
                catch (Exception ex)
                {
                    AppendProgress($"Error renaming file: {fileName}. Error: {ex.Message}");
                }

                fileCount++;
            }

            AppendProgress("All MP3 files have been renamed.");
        }

        private async Task DeleteWavFilesAsync(string[] wavFiles)
        {
            AppendProgress("Deleting WAV files...");

            await Task.Run(() =>
            {
                int fileCount = 1;
                foreach (var wavFile in wavFiles)
                {
                    try
                    {
                        // Update progress without showing file names
                        AppendProgress($"Deleting file {fileCount}/{wavFiles.Length}...", overwrite: true);

                        File.Delete(wavFile);
                    }
                    catch (Exception ex)
                    {
                        AppendProgress($"Error deleting file: {Path.GetFileName(wavFile)}. Error: {ex.Message}");
                    }

                    fileCount++;
                }
            });

            AppendProgress("All WAV files have been deleted.");
        }

        private void AppendProgress(string message, bool overwrite = false)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendProgress(message, overwrite)));
                return;
            }

            if (overwrite && progressTextBox.Lines.Length > 0)
            {
                // Overwrite the last line
                var lines = progressTextBox.Lines.ToList();
                lines[lines.Count - 1] = message;
                progressTextBox.Lines = lines.ToArray();
            }
            else
            {
                // Add a new line
                progressTextBox.AppendText(message + Environment.NewLine);
            }

            // Scroll to the bottom to ensure the user sees the latest update
            progressTextBox.SelectionStart = progressTextBox.Text.Length;
            progressTextBox.ScrollToCaret();
        }

        private async Task ValidateCallsAsync(string folderPath, DateTime folderDate)
        {
            // Prompt the user to select the CSV file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Select CDR CSV File"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            string csvFilePath = openFileDialog.FileName;
            string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            string outputFilePath = Path.Combine(downloadsFolder, $"CDR Compare {folderDate:yyyy-MM-dd}.csv");

            try
            {
                // Read the CSV file
                var csvLines = await File.ReadAllLinesAsync(csvFilePath);
                if (csvLines.Length == 0) throw new Exception("The CSV file is empty.");

                // Extract "Recording File" column
                var header = csvLines[0].Split(',');
                int recordingFileIndex = Array.IndexOf(header, "Recording File");
                if (recordingFileIndex == -1) throw new Exception("The CSV file does not contain a 'Recording File' column.");

                var validRows = new List<string> { csvLines[0] }; // Add header
                var csvFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Filter rows with valid "Recording File" and remove empty ones
                for (int i = 1; i < csvLines.Length; i++)
                {
                    var columns = csvLines[i].Split(',');
                    if (columns.Length > recordingFileIndex && !string.IsNullOrWhiteSpace(columns[recordingFileIndex]))
                    {
                        string recordingFile = columns[recordingFileIndex].Replace(".yswav", "").Trim();
                        validRows.Add(csvLines[i]); // Add only valid rows to the list
                        csvFileNames.Add(recordingFile);
                    }
                }

                // Write filtered rows back to a new CSV for debugging (optional)
                string filteredCsvPath = Path.Combine(downloadsFolder, $"Filtered_CDR_{folderDate:yyyy-MM-dd}.csv");
                await File.WriteAllLinesAsync(filteredCsvPath, validRows);

                // Compare with MP3 files
                var mp3Files = Directory.GetFiles(folderPath, "*.mp3").Select(Path.GetFileNameWithoutExtension).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var missingFiles = csvFileNames.Except(mp3Files).ToList();

                // Debug Logging
                AppendProgress($"Total Files in CSV (After Filtering): {csvFileNames.Count}");
                AppendProgress($"Total MP3 Files in Folder: {mp3Files.Count}");
                AppendProgress($"Missing Files Count: {missingFiles.Count}");

                // Write missing files to new CSV
                if (missingFiles.Count > 0)
                {
                    var outputLines = new List<string> { "Missing Recording File" };
                    outputLines.AddRange(missingFiles);
                    await File.WriteAllLinesAsync(outputFilePath, outputLines);

                    MessageBox.Show($"Comparison complete. Missing files saved to: {outputFilePath}", "Validation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("All files in the CSV were found in the folder.", "Validation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating calls: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region Faults + Servers

        private void LoadHosts()
        {
            // Replace this with your actual data
            hostList.Add(new HostData { Hostname = "Google", IPAddress = "8.8.8.8", CheckUptime = false });
            hostList.Add(new HostData { Hostname = "DC01", IPAddress = "able-dc01", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "DC02", IPAddress = "able-dc02", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "FS01", IPAddress = "able-fs01", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "FS02", IPAddress = "able-fs02", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "FS03", IPAddress = "able-fs03", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "Veeam01", IPAddress = "able-veeam01", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "Backup01", IPAddress = "able-backup01", CheckUptime = true });
            hostList.Add(new HostData { Hostname = "Fortinet", IPAddress = "192.168.0.10", CheckUptime = false });
            hostList.Add(new HostData { Hostname = "VM Host 1", IPAddress = "192.168.0.26", CheckUptime = false });
            hostList.Add(new HostData { Hostname = "VM Host 2", IPAddress = "192.168.0.25", CheckUptime = false });
            hostList.Add(new HostData { Hostname = "Server SQL", IPAddress = "ABLE-SERVER-SQL", CheckUptime = false });

            hostList.Add(new HostData
            {
                Hostname = "Elliot's SQL Server",
                IPAddress = "Bananagoats.co.uk",
                IsSqlServer = true,
                Username = "Elliot",
                Password = "1234",
                DatabaseName = "Ableworld",
                CheckUptime = false
            });

            hostList.Add(new HostData { Hostname = "HANA Server", IPAddress = "10.100.230.6", CheckUptime = false });
            hostList.Add(new HostData { Hostname = "FSM Server", IPAddress = "10.100.230.50", CheckUptime = false });
        }

        private async Task<bool> CheckSqlConnectionAsync(HostData host)
        {
            try
            {
                string connectionString = $"Server={host.IPAddress};Database={host.DatabaseName};User Id={host.Username};Password={host.Password};";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true; // Connection successful
                }
            }
            catch
            {
                return false; // Connection failed
            }
        }

        private void InitializePingTimer()
        {
            pingTimer = new Timer();
            pingTimer.Interval = 60 * 1000; // 30 seconds
            pingTimer.Tick += async (s, e) => await PingHostsAsync();
            pingTimer.Start();
        }

        private string GetHostUptime(string ipAddress)
        {
            try
            {
                // Use WMI to query system uptime remotely
                var scope = new ManagementScope($@"\\{ipAddress}\root\cimv2");
                var query = new ObjectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem");
                var searcher = new ManagementObjectSearcher(scope, query);

                scope.Connect(); // Connect to the remote host

                foreach (var obj in searcher.Get())
                {
                    var lastBootUpTime = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                    var uptime = DateTime.Now - lastBootUpTime;

                    return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m";
                }
            }
            catch (Exception ex)
            {
                // Handle cases where uptime cannot be retrieved
                return "Unknown";
            }

            return "Unknown";
        }

        private void PopulateDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = hostList;

            // Clear and redefine columns
            dataGridView1.Columns.Clear();

            // Host Name Column
            dataGridView1.Columns.Add("Hostname", "Host Name");
            dataGridView1.Columns["Hostname"].DataPropertyName = "Hostname";

            // Uptime Column
            dataGridView1.Columns.Add("Uptime", "Uptime");
            dataGridView1.Columns["Uptime"].DataPropertyName = "Uptime";

            // Set DataGridView to be read-only and non-selectable
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            // Disable selection highlight
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

            // Prevent row selection
            dataGridView1.SelectionChanged += (s, e) => dataGridView1.ClearSelection();
        }

        private async Task PingHostsAsync()
        {
            foreach (var host in hostList)
            {
                if (host.IsSqlServer)
                {
                    // Check SQL Server connection and set Uptime to N/A
                    host.IsOnline = await CheckSqlConnectionAsync(host);
                    host.Uptime = "N/A";
                }
                else
                {
                    // Ping the host
                    host.IsOnline = await PingHostAsync(host.IPAddress);

                    // Check uptime only if CheckUptime is true
                    if (host.CheckUptime && host.IsOnline)
                    {
                        host.Uptime = GetHostUptime(host.IPAddress);
                    }
                    else
                    {
                        host.Uptime = host.IsOnline ? "Online" : "Offline";
                    }
                }

                // Update the row color and Uptime immediately after the check
                UpdateRowColor(host);
            }
        }

        private async Task<bool> PingHostAsync(string ipAddress)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    var result = await ping.SendPingAsync(ipAddress, 2000); // 2-second timeout
                    return result.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateRowColor(HostData host)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateRowColor(host)));
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DataBoundItem is HostData data && data == host)
                {
                    row.DefaultCellStyle.BackColor = host.IsOnline ? Color.YellowGreen : Color.Tomato;
                }
            }
        }

        private void InitializeTimer()
        {
            Timer timerRefresh = new Timer();
            timerRefresh.Interval = 5 * 1000;
            timerRefresh.Tick += timerRefresh_Tick;
            timerRefresh.Start();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            _ = LoadFaultsAsync();
        }

        private async Task LoadFaultsAsync()
        {
            var data = await GetDataFromDatabaseAsync();
            var faultList = GetFaults(data);
            PopulateDataGridView(faultList);
        }

        private void PopulateDataGridView(List<MachineFault> faults)
        {
            var sortedFaults = faults.OrderBy(f => f.Location).ToList();

            dataGridViewFaults.DataSource = sortedFaults;
            dataGridViewFaults.ClearSelection();
        }

        private async Task<List<MachineData>> GetDataFromDatabaseAsync()
        {
            var machineDataList = new List<MachineData>();

            // Replace with your actual SQL Server connection string
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT MachineName, Location, WANIP, ISP, CPUInfo, RAMInfo, StorageInfo, WindowsOS, BuildNumber, 
                       DateTimeReceived, SenderVersion, PendingUpdates, LatestSharepointFile
                FROM MachineData";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var machine = new MachineData
                                {
                                    MachineName = reader["MachineName"] as string,
                                    Location = reader["Location"] as string,
                                    WANIP = reader["WANIP"] as string,
                                    ISP = reader["ISP"] as string,
                                    CPUInfo = reader["CPUInfo"] as string,
                                    RAMInfo = reader["RAMInfo"] as string,
                                    StorageInfo = reader["StorageInfo"] as string,
                                    WindowsOS = reader["WindowsOS"] as string,
                                    BuildNumber = reader["BuildNumber"] as string,
                                    DateTimeReceived = reader["DateTimeReceived"] as DateTime?,
                                    SenderVersion = reader["SenderVersion"] as string,
                                    PendingUpdates = reader["PendingUpdates"] as string,
                                    LatestSharepointFile = reader["LatestSharepointFile"] as DateTime?
                                };

                                machineDataList.Add(machine);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return machineDataList;
        }

        private List<MachineFault> GetFaults(List<MachineData> machineDataList)
        {
            var faults = new List<MachineFault>();

            foreach (var machine in machineDataList)
            {
                var machineFaults = CheckMachineForFaults(machine);
                foreach (var fault in machineFaults)
                {
                    faults.Add(new MachineFault
                    {
                        MachineName = machine.MachineName,
                        Location = machine.Location,
                        Fault = fault
                    });
                }
            }

            return faults;
        }

        private List<string> CheckMachineForFaults(MachineData machine)
        {
            var faults = new List<string>();
            DateTime now = DateTime.Now;

            // Connectivity Fault
            if (machine.DateTimeReceived.HasValue && (now - machine.DateTimeReceived.Value).TotalMinutes > 5)
            {
                faults.Add("Machine is offline.");
            }

            // Storage Fault
            if (!string.IsNullOrEmpty(machine.StorageInfo))
            {
                var storageFault = CheckStorageFault(machine.StorageInfo);
                if (!string.IsNullOrEmpty(storageFault))
                {
                    faults.Add(storageFault);
                }
            }

            if (!string.IsNullOrEmpty(machine.WindowsOS) && !machine.WindowsOS.Equals("Windows 11 Pro", StringComparison.OrdinalIgnoreCase))
            {
                faults.Add($"Operating system is {machine.WindowsOS}.");
            }

            // Additional Fault Criteria can be added here

            return faults;
        }

        private string CheckStorageFault(string storageInfo)
        {
            try
            {
                // Expected format: "Used/Max"
                var parts = storageInfo.Replace("GB", "").Trim().Split('/');
                if (parts.Length == 2)
                {
                    if (double.TryParse(parts[0], out double used) && double.TryParse(parts[1], out double max))
                    {
                        if (max > 0)
                        {
                            double usagePercent = (used / max) * 100;
                            if (usagePercent >= 80)
                            {
                                return $"Storage usage is {usagePercent:F2}% of max capacity.";
                            }
                        }
                    }
                }
            }
            catch
            {
                // Handle parsing errors if necessary
            }

            return null;
        }

        #endregion



        // New class to hold fault data
        public class MachineFault
        {
            public string MachineName { get; set; }
            public string Location { get; set; }
            public string Fault { get; set; }
        }

        public class MachineData
        {
            public string MachineName { get; set; }
            public string Location { get; set; }
            public string WANIP { get; set; }
            public string ISP { get; set; }
            public string CPUInfo { get; set; }
            public string RAMInfo { get; set; }
            public string StorageInfo { get; set; }
            public string WindowsOS { get; set; }
            public string BuildNumber { get; set; }
            public DateTime? DateTimeReceived { get; set; }
            public string SenderVersion { get; set; }
            public string PendingUpdates { get; set; }
            public DateTime? LatestSharepointFile { get; set; }
        }

        public class HostData
        {
            public string Hostname { get; set; }
            public string IPAddress { get; set; }
            public bool IsOnline { get; set; } // Indicates if the host is reachable
            public bool IsSqlServer { get; set; }
            public string Username { get; set; } // SQL Server username
            public string Password { get; set; }
            public string DatabaseName { get; set; }
            public string Uptime { get; set; } // Store uptime or "N/A"
            public bool CheckUptime { get; set; } // Indicates whether to check uptime
        }
    }
}
