using Microsoft.Office.Core;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using BG_Menu.Data;
using System.Drawing;
using System.IO; // For file operations
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Drawing;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class DashBoard : Form
    {
        private List<HostData> hostList = new List<HostData>();
        private Timer pingTimer;
        private SalesRepository salesRepository = new SalesRepository();

        private List<LogEntry> logEntries = new List<LogEntry>();
        private int currentLogIndex = -1;

        public DashBoard()
        {
            InitializeComponent();
        }

        private async void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;

                LoadHosts();
                PopulateDataGridView();

                _ = PingHostsAsync();

                InitializePingTimer();
                InitializeTimer();

                await LoadFoldersAsync();
                await LoadFaultsAsync();
                UpdateListViewData();
            }
            catch (Exception ex)
            {

            }
        }

        #region Call Recordings
        private async Task LoadFoldersAsync()
        {
            try
            {
                string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";


                var directories = await Task.Run(() =>
                {
                    if (!Directory.Exists(targetDirectory))
                    {
                        return new List<string>();
                    }
                    return Directory.GetDirectories(targetDirectory)
                                    .Where(dir => Path.GetFileName(dir).StartsWith("RecordingFiles-"))
                                    .ToList();
                });

                // Update the UI on the main thread.
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
            catch
            {
                // Silently ignore errors (e.g. folder not found).
            }
        }

        private async void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                progressTextBox.Clear();

                // Check if the clicked cell is in the button column (assumed index 1)
                if (e.ColumnIndex == 1)
                {
                    string targetDirectory = @"\\marketingnas\Phone System\Recordings ( PBX Archive )";
                    string folderName = dataGridView2.Rows[e.RowIndex].Cells[0].Value?.ToString();

                    if (string.IsNullOrEmpty(folderName))
                    {
                        MessageBox.Show("Folder name is empty. Please check the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string fullPath = Path.Combine(targetDirectory, folderName);

                    if (!Directory.Exists(fullPath))
                    {
                        MessageBox.Show($"The directory '{fullPath}' does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        await LoadFoldersAsync();
                        return;
                    }

                    string newName = GetNewFolderName(folderName);
                    if (string.IsNullOrEmpty(newName))
                    {
                        MessageBox.Show("Failed to generate a new name for the folder. Check the folder format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string newFullPath = Path.Combine(targetDirectory, newName);

                    try
                    {
                        // Rename the folder
                        Directory.Move(fullPath, newFullPath);
                        AppendProgress($"Folder renamed to: {newName}");

                        // Process all WAV files sequentially.
                        await ProcessWavFilesSequentiallyAsync(newFullPath);

                        // Refresh folder list.
                        await LoadFoldersAsync();
                    }
                    catch (Exception ex)
                    {
                        AppendProgress($"Error during operations: {ex.Message}");
                        MessageBox.Show($"Error during operations:\nSource Path: {fullPath}\nDestination Path: {newFullPath}\nError: {ex.Message}",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred in the cell click event: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetNewFolderName(string folderName)
        {
            var parts = folderName.Split('-');
            if (parts.Length < 3) return null;

            string datePart = parts.Last();
            if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
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


        private string GetNewMp3FileName(string wavFilePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(wavFilePath);
            string[] parts = fileName.Split('-');
            if (parts.Length >= 5 && DateTime.TryParseExact(parts[0], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime timestamp))
            {
                string formattedTimestamp = timestamp.ToString("dd-MM-yyyy HH-mm");
                string caller = parts[2];
                string callee = parts[3];
                return $"{formattedTimestamp} ( {caller} - {callee} ).mp3";
            }
            else
            {
                // Fallback if file name format is not as expected.
                return fileName + ".mp3";
            }
        }

        class LogEntry
        {
            public string Message { get; set; }
            public Color Color { get; set; }
        }

        private async Task ProcessWavFilesSequentiallyAsync(string folderPath)
        {
            var wavFiles = Directory.GetFiles(folderPath, "*.wav");
            int totalFiles = wavFiles.Length;
            int counter = 0;

            // Clear any previous log entries.
            logEntries.Clear();
            currentLogIndex = -1;
            RefreshLogDisplay();

            foreach (var wavFile in wavFiles)
            {
                counter++;

                var (alreadyProcessed, newFileName) = await ProcessWavFileAsync(wavFile);
                string message = $"{counter} / {totalFiles} - {newFileName} " + (alreadyProcessed ? "File already Processed" : "Processed");

                if (alreadyProcessed)
                {
                    logEntries.Add(new LogEntry { Message = message, Color = Color.Red });

                    currentLogIndex = -1;
                }
                else
                {

                    if (currentLogIndex == -1)
                    {
                        logEntries.Add(new LogEntry { Message = message, Color = Color.YellowGreen });
                        currentLogIndex = logEntries.Count - 1;
                    }
                    else
                    {
                        logEntries[currentLogIndex].Message = message;
                    }
                }
                RefreshLogDisplay();
            }

            logEntries.Add(new LogEntry { Message = "All files processed", Color = Color.Green });
            RefreshLogDisplay();
        }

        private void RefreshLogDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshLogDisplay));
                return;
            }

            progressTextBox.Clear();
            foreach (var entry in logEntries)
            {
                progressTextBox.SelectionStart = progressTextBox.TextLength;
                progressTextBox.SelectionColor = entry.Color;
                progressTextBox.AppendText(entry.Message + Environment.NewLine);
            }
            progressTextBox.SelectionColor = progressTextBox.ForeColor;
            progressTextBox.ScrollToCaret();
        }

        private async Task<(bool alreadyProcessed, string newFileName)> ProcessWavFileAsync(string wavFile)
        {
            // Determine the intended new MP3 file name from the original file.
            string newFileName = GetNewMp3FileName(wavFile);
            string processingFile = wavFile + ".processing";
            try
            {
                // Attempt to lock the file.
                File.Move(wavFile, processingFile);
            }
            catch (Exception)
            {
                // If we cannot lock it, assume it has already been processed.
                return (true, newFileName);
            }

            string mp3File = Path.ChangeExtension(processingFile, ".mp3");
            try
            {
                using (var reader = new NAudio.Wave.AudioFileReader(processingFile))
                using (var writer = new NAudio.Lame.LameMP3FileWriter(mp3File, reader.WaveFormat, NAudio.Lame.LAMEPreset.VBR_90))
                {
                    await Task.Run(() => reader.CopyTo(writer));
                }

                // Rename the MP3 file to the new name.
                string finalMp3FilePath = Path.Combine(Path.GetDirectoryName(mp3File), newFileName);
                File.Move(mp3File, finalMp3FilePath);
            }
            catch (Exception)
            {
                // If an error occurs during conversion, you can log or handle it as needed.
            }
            finally
            {
                // Ensure that the processing file is deleted.
                if (File.Exists(processingFile))
                {
                    try { File.Delete(processingFile); } catch { }
                }
            }

            return (false, newFileName);
        }


        private void AppendProgress(string message, bool overwrite = false, Color? textColor = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendProgress(message, overwrite, textColor)));
                return;
            }

            // Ensure you're using a RichTextBox control.
            RichTextBox richTextBox = progressTextBox as RichTextBox;
            if (richTextBox != null)
            {
                if (textColor.HasValue)
                {
                    richTextBox.SelectionStart = richTextBox.TextLength;
                    richTextBox.SelectionLength = 0;
                    richTextBox.SelectionColor = textColor.Value;
                }

                if (overwrite && richTextBox.Lines.Length > 0)
                {
                    var lines = richTextBox.Lines.ToList();
                    lines[lines.Count - 1] = message;
                    richTextBox.Lines = lines.ToArray();
                }
                else
                {
                    richTextBox.AppendText(message + Environment.NewLine);
                }

                // Reset the color to default.
                richTextBox.SelectionColor = richTextBox.ForeColor;
                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
            }
            else
            {
                // Fallback for a plain TextBox (without color)
                progressTextBox.AppendText(message + Environment.NewLine);
            }
        }

        #endregion

        #region Faults + Servers

        private void LoadHosts()
        {
            try
            {
                // Replace with actual host data.
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
                hostList.Add(new HostData { Hostname = "HANA Server", IPAddress = "10.100.230.6", CheckUptime = false });
                hostList.Add(new HostData { Hostname = "FSM Server", IPAddress = "10.100.230.50", CheckUptime = false });
            }
            catch
            {
                // Silently handle any errors loading hosts.
            }
        }

        private async Task<bool> CheckSqlConnectionAsync(HostData host)
        {
            try
            {
                string connectionString = $"Server={host.IPAddress};Database={host.DatabaseName};User Id={host.Username};Password={host.Password};";
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void InitializePingTimer()
        {
            pingTimer = new Timer();
            pingTimer.Interval = 60 * 1000; // 60 seconds
            pingTimer.Tick += async (s, e) =>
            {
                try
                {
                    await PingHostsAsync();
                }
                catch
                {
                    // Silently handle any ping errors.
                }
            };
            pingTimer.Start();
        }

        private string GetHostUptime(string ipAddress)
        {
            try
            {
                var scope = new ManagementScope($@"\\{ipAddress}\root\cimv2");
                var query = new ObjectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem");
                var searcher = new ManagementObjectSearcher(scope, query);
                scope.Connect();

                foreach (var obj in searcher.Get())
                {
                    var lastBootUpTime = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                    var uptime = DateTime.Now - lastBootUpTime;
                    return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m";
                }
            }
            catch
            {
                return "Unknown";
            }
            return "Unknown";
        }

        private void PopulateDataGridView()
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = hostList;

                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("Hostname", "Host Name");
                dataGridView1.Columns["Hostname"].DataPropertyName = "Hostname";

                dataGridView1.Columns.Add("Uptime", "Uptime");
                dataGridView1.Columns["Uptime"].DataPropertyName = "Uptime";

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;

                dataGridView1.ClearSelection();
                dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
                dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

                dataGridView1.SelectionChanged += (s, e) => dataGridView1.ClearSelection();
            }
            catch
            {
                // Silently handle errors in populating the grid.
            }
        }

        private async Task PingHostsAsync()
        {
            try
            {
                foreach (var host in hostList)
                {
                    if (host.IsSqlServer)
                    {
                        host.IsOnline = await CheckSqlConnectionAsync(host);
                        host.Uptime = "N/A";
                    }
                    else
                    {
                        host.IsOnline = await PingHostAsync(host.IPAddress);
                        if (host.CheckUptime && host.IsOnline)
                        {
                            host.Uptime = GetHostUptime(host.IPAddress);
                        }
                        else
                        {
                            host.Uptime = host.IsOnline ? "Online" : "Offline";
                        }
                    }
                    UpdateRowColor(host);
                }
            }
            catch
            {
                // Silently handle any errors during ping operations.
            }
        }

        private async Task<bool> PingHostAsync(string ipAddress)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    var result = await ping.SendPingAsync(ipAddress, 2000);
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
            timerRefresh.Interval = 60 * 1000;
            timerRefresh.Tick += async (sender, e) =>
            {
                try
                {
                    await LoadFaultsAsync();
                }
                catch
                {
                    // Silently handle exceptions.
                }
            };
            timerRefresh.Start();
        }

        private async Task LoadFaultsAsync()
        {
            try
            {
                var data = await Task.Run(() => salesRepository.ExecuteSqlQuery(@"
                    SELECT 
                        Name AS MachineName, 
                        Store AS Location, 
                        CPU AS CPUInfo, 
                        Ram AS RAMInfo, 
                        HHD AS StorageInfo, 
                        OS AS WindowsOS, 
                        OS_Version AS BuildNumber, 
                        OS_Updates AS PendingUpdates,
                        Pulse_Time AS DateTimeReceived, 
                        Client_Version AS SenderVersion,
                        'TBPC' AS TableSource
                    FROM TBPC
                    UNION ALL
                    SELECT 
                        Name AS MachineName, 
                        Store AS Location, 
                        CPU AS CPUInfo, 
                        Ram AS RAMInfo, 
                        HHD AS StorageInfo, 
                        OS AS WindowsOS, 
                        OS_Version AS BuildNumber, 
                        NULL AS PendingUpdates,
                        NULL AS DateTimeReceived, 
                        NULL AS SenderVersion,
                        'HOPC' AS TableSource
                    FROM HOPC;"));

                var machineDataList = new List<MachineData>();
                foreach (DataRow row in data.Rows)
                {
                    var machine = new MachineData
                    {
                        MachineName = row["MachineName"] as string,
                        Location = row["Location"] as string,
                        CPUInfo = row["CPUInfo"] as string,
                        RAMInfo = row["RAMInfo"] as string,
                        StorageInfo = row["StorageInfo"] as string,
                        WindowsOS = row["WindowsOS"] as string,
                        BuildNumber = row["BuildNumber"] as string,
                        DateTimeReceived = row["DateTimeReceived"] as DateTime?,
                        SenderVersion = row["SenderVersion"] as string,
                        PendingUpdates = row["PendingUpdates"] as string,
                        TableSource = row["TableSource"] as string
                    };
                    machineDataList.Add(machine);
                }
                var faultList = GetFaults(machineDataList);
                PopulateDataGridView(faultList);
            }
            catch { }
        }

        private void PopulateDataGridView(List<MachineFault> faults)
        {
            try
            {
                var sortedFaults = faults.OrderBy(f => f.Location).ToList();
                dataGridViewFaults.DataSource = sortedFaults;
                dataGridViewFaults.ClearSelection();
            }
            catch
            {
                // Silently handle errors.
            }
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

            if (machine.TableSource == "TBPC")
            {
                if (machine.DateTimeReceived.HasValue &&
                    (now - machine.DateTimeReceived.Value).TotalMinutes > 5 &&
                    !string.IsNullOrEmpty(machine.Location) &&
                    machine.Location.IndexOf("till", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    faults.Add("Machine is offline.");
                }
            }

            if (!string.IsNullOrEmpty(machine.StorageInfo))
            {
                var storageFault = CheckStorageFault(machine.StorageInfo);
                if (!string.IsNullOrEmpty(storageFault))
                {
                    faults.Add(storageFault);
                }
            }

            if (!string.IsNullOrEmpty(machine.WindowsOS) &&
                !machine.WindowsOS.Equals("Windows 11 Pro", StringComparison.OrdinalIgnoreCase))
            {
                faults.Add($"Operating system is {machine.WindowsOS}.");
            }

            if (IsEligibleForLowRamCheck(machine.MachineName) &&
                (string.IsNullOrEmpty(machine.Location) || machine.Location.IndexOf("Workshop", StringComparison.OrdinalIgnoreCase) < 0))
            {
                double ramInGB = ParseRamInfo(machine.RAMInfo);
                if (ramInGB > 0 && ramInGB < 9)
                {
                    faults.Add($"Low RAM: {ramInGB} GB available.");
                }
            }

            return faults;
        }

        private string CheckStorageFault(string storageInfo)
        {
            try
            {
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
                // Ignore parsing errors.
            }
            return null;
        }

        private double ParseRamInfo(string ramInfo)
        {
            if (string.IsNullOrWhiteSpace(ramInfo))
                return -1;

            string numericPart = new string(ramInfo.Where(c => char.IsDigit(c) || c == '.').ToArray());
            if (double.TryParse(numericPart, out double ramInGB))
            {
                return ramInGB;
            }
            return -1;
        }

        private bool IsEligibleForLowRamCheck(string machineName)
        {
            if (string.IsNullOrEmpty(machineName))
                return false;

            return machineName.StartsWith("ABL0", StringComparison.OrdinalIgnoreCase) ||
                   machineName.StartsWith("WS-", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region DisplaySums

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var selectedItem = listView1.SelectedItems[0];
            string detailQuery = selectedItem.Tag as string;

            if (!string.IsNullOrEmpty(detailQuery))
            {
                try
                {
                    Form detailForm = new Form
                    {
                        Text = "Detail View",
                        Width = 600,
                        Height = 400,
                        StartPosition = FormStartPosition.CenterParent
                    };

                    DataGridView dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        AllowUserToAddRows = false,
                        ReadOnly = true,
                        RowHeadersVisible = false,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    };

                    detailForm.Controls.Add(dgv);

                    string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(detailQuery, connection))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgv.DataSource = dt;
                        }
                    }

                    detailForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error displaying detail data:\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AddRowToListView(string description, string query, string detailQuery)
        {
            try
            {
                SetupListView();
                DataTable dt = salesRepository.ExecuteSqlQuery(query);
                if (dt.Rows.Count > 0)
                {
                    int count1 = dt.Rows[0].IsNull(0) ? 0 : Convert.ToInt32(dt.Rows[0][0]);
                    int count2 = dt.Rows[0].IsNull(1) ? 0 : Convert.ToInt32(dt.Rows[0][1]);

                    var item = new ListViewItem(description);
                    item.SubItems.Add($"{count2} / {count1}");
                    item.Tag = detailQuery;
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupListView(string column1Name = "Description", string column2Name = "Count")
        {
            if (listView1.Columns.Count == 0)
            {
                listView1.Clear();
                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                // Fully qualify HorizontalAlignment to resolve ambiguity.
                listView1.Columns.Add(column1Name, 200, System.Windows.Forms.HorizontalAlignment.Left);
                listView1.Columns.Add(column2Name, 150, System.Windows.Forms.HorizontalAlignment.Left);
            }
        }

        public void UpdateListViewData()
        {
            string query1 = @"
            SELECT
                SUM(CASE WHEN Name LIKE '%ABL%' THEN 1 ELSE 0 END) AS [RowsWithABL],
                SUM(CASE WHEN OS_Updates IS NOT NULL AND OS_Updates NOT LIKE '%No%' THEN 1 ELSE 0 END) AS [RowsWithValidPendingUpdates]
            FROM [TBPC];";

            string detailQuery1 = @"
            SELECT Name AS MachineName, Store AS Location, CPU AS CPUInfo, Ram AS RAMInfo, HHD AS StorageInfo, 
                   OS AS WindowsOS, OS_Version AS BuildNumber, OS_Updates AS PendingUpdates
            FROM [TBPC]
            WHERE OS_Updates IS NOT NULL AND OS_Updates NOT LIKE '%None%';";

            AddRowToListView("Store Pending Updates", query1, detailQuery1);

            string query2 = @"
            SELECT
                SUM(CASE WHEN Name LIKE 'ABLL%' THEN 1 ELSE 0 END) AS [RowsWithABLL],
                SUM(CASE WHEN ISNUMERIC(REPLACE(Ram, 'GB', '')) = 1 AND CAST(REPLACE(Ram, 'GB', '') AS INT) < 7 THEN 1 ELSE 0 END) AS [RowsWithLowRam]
            FROM [TBPC];";

            string detailQuery2 = @"
            SELECT Name AS MachineName, Store AS Location, CPU AS CPUInfo, Ram AS RAMInfo, HHD AS StorageInfo, 
                   OS AS WindowsOS, OS_Version AS BuildNumber, OS_Updates AS PendingUpdates
            FROM [TBPC]
            WHERE Name LIKE 'ABLL%' AND ISNUMERIC(REPLACE(Ram, 'GB', '')) = 1 AND CAST(REPLACE(Ram, 'GB', '') AS INT) < 7;";
            AddRowToListView("Unusable Queuebusters", query2, detailQuery2);

            string query3 = @"
            SELECT
                COUNT(*) AS [TotalRows],
                SUM(CASE WHEN AppUpdate = 'Yes' THEN 1 ELSE 0 END) AS [RowsWithAppUpdateYes]
            FROM [Computers]
            WHERE MachineName LIKE '%ABL%';";

            string detailQuery3 = @"
            SELECT MachineName, Location, Company, Type, AppUpdate, TillUpdater, CallPopLite
            FROM [Computers]
            WHERE AppUpdate = 'Yes' AND MachineName LIKE '%ABL%';";
            AddRowToListView("AppUpdate Status", query3, detailQuery3);

            string query4 = @"
            SELECT
                SUM(CASE WHEN Name LIKE '%ABL%' THEN 1 ELSE 0 END) AS [TotalRowsWithABL],
                SUM(CASE WHEN Name LIKE '%ABL%' AND OS_Version LIKE '%26100%' THEN 1 ELSE 0 END) AS [RowsWithBuildNumber26100AndABL]
            FROM [TBPC];";

            string detailQuery4 = @"
            SELECT Name AS MachineName, Store AS Location, CPU AS CPUInfo, Ram AS RAMInfo, HHD AS StorageInfo, 
                   OS AS WindowsOS, OS_Version AS BuildNumber, Client_Version AS SenderVersion
            FROM [TBPC]
            WHERE Name LIKE 'ABL%' AND OS_Version NOT LIKE '%26100%';";
            AddRowToListView("Store Machines on 24H2", query4, detailQuery4);

            string query5 = @"
            SELECT
                SUM(CASE WHEN Name LIKE '%WS-%' THEN 1 ELSE 0 END) AS [TotalRowsWithWS],
                SUM(CASE WHEN Name LIKE '%WS-%' AND OS_Version LIKE '%26100%' THEN 1 ELSE 0 END) AS [RowsWithBuildNumber26100AndWS]
            FROM [HOPC];";

            string detailQuery5 = @"
            SELECT Name AS MachineName, Store AS Location, CPU AS CPUInfo, Ram AS RAMInfo, HHD AS StorageInfo, 
                   OS AS WindowsOS, OS_Version AS BuildNumber
            FROM [HOPC]
            WHERE Name LIKE 'WS-%' AND OS_Version NOT LIKE '%26100%';";
            AddRowToListView("Head Office Machines on 24H2", query5, detailQuery5);
        }

        #endregion

        // Data classes

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
            public string WANIP { get; set; } // Will be null for HOPC
            public string ISP { get; set; }   // Will be null for HOPC
            public string CPUInfo { get; set; }
            public string RAMInfo { get; set; }
            public string StorageInfo { get; set; }
            public string WindowsOS { get; set; }
            public string BuildNumber { get; set; }
            public DateTime? DateTimeReceived { get; set; } // Will be null for HOPC
            public string SenderVersion { get; set; }
            public string PendingUpdates { get; set; }
            public DateTime? LatestSharepointFile { get; set; }
            public string TableSource { get; set; } // Indicates whether the data is from TBPC or HOPC
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
