using Microsoft.Office.Core;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using PdfSharp.Charting;
using PdfSharp.Snippets;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            InitializeMissingDatesGrid();

            Task.Run(CheckCallRecordingGapsOptimized);

            // Perform the first ping on form load
            Task.Run(async () => await PingHostsAsync());

            InitializePingTimer();
            InitializeTimer();
        }

        private void LoadHosts()
        {
            // Replace this with your actual data
            hostList.Add(new HostData { Hostname = "Google", IPAddress = "8.8.8.8" });
            hostList.Add(new HostData { Hostname = "DC01", IPAddress = "able-dc01" });
            hostList.Add(new HostData { Hostname = "DC02", IPAddress = "able-dc02" });
            hostList.Add(new HostData { Hostname = "FS01", IPAddress = "able-fs01" });
            hostList.Add(new HostData { Hostname = "FS02", IPAddress = "able-fs02" });
            hostList.Add(new HostData { Hostname = "FS03", IPAddress = "able-fs03" });
            //hostList.Add(new HostData { Hostname = "REC01", IPAddress = "able-rec" });
            hostList.Add(new HostData { Hostname = "Veeam01", IPAddress = "able-veeam01" });
            hostList.Add(new HostData { Hostname = "Backup01", IPAddress = "able-backup01" });
            hostList.Add(new HostData { Hostname = "Fortinet", IPAddress = "192.168.0.10" });
            hostList.Add(new HostData { Hostname = "VM Host 1", IPAddress = "192.168.0.26" });
            hostList.Add(new HostData { Hostname = "VM Host 2", IPAddress = "192.168.0.25" });
            hostList.Add(new HostData { Hostname = "Server SQL", IPAddress = "ABLE-SERVER-SQL" });
            hostList.Add(new HostData { Hostname = "Unifi Controller", IPAddress = "192.168.0.50" });

            hostList.Add(new HostData
            {
                Hostname = "Elliot's SQL Server",
                IPAddress = "Bananagoats.co.uk",
                IsSqlServer = true,
                Username = "Elliot",
                Password = "1234",
                DatabaseName = "Ableworld"
            });

            hostList.Add(new HostData { Hostname = "HANA Server", IPAddress = "10.100.230.6" });
            hostList.Add(new HostData { Hostname = "FSM Server", IPAddress = "10.100.230.50" });

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
            pingTimer.Interval = 30 * 1000; // 30 seconds
            pingTimer.Tick += async (s, e) => await PingHostsAsync();
            pingTimer.Start();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = hostList;

            // Ensure only one column is displayed for Hostname
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Hostname", "Host Name");
            dataGridView1.Columns["Hostname"].DataPropertyName = "Hostname";

            // Set DataGridView to be read-only and non-selectable
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            // Disable the selection
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

            // Handle selection changed to prevent selection
            dataGridView1.SelectionChanged += (s, e) => dataGridView1.ClearSelection();
        }

        private async Task PingHostsAsync()
        {
            foreach (var host in hostList)
            {
                if (host.IsSqlServer)
                {
                    // Check SQL Server connection with specified database
                    host.IsOnline = await CheckSqlConnectionAsync(host);
                }
                else
                {
                    // Ping the host
                    host.IsOnline = await PingHostAsync(host.IPAddress);
                }

                // Update the row color immediately after the check
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
            string connectionString = "Server=bananagoats.co.uk;Database=Ableworld;User Id=Elliot;Password=1234;";

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
                MessageBox.Show($"Error retrieving data from database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void InitializeMissingDatesGrid()
        {
            dataGridViewMissingDates.AutoGenerateColumns = false;
            dataGridViewMissingDates.Columns.Clear();

            // Add a single column for "Missing Call Recording Dates"
            var column = new DataGridViewTextBoxColumn
            {
                Name = "MissingDates",
                HeaderText = "Missing Call Recording Dates",
                DataPropertyName = "MissingDates",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridViewMissingDates.Columns.Add(column);

            // Ensure no row is selected
            dataGridViewMissingDates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewMissingDates.MultiSelect = false;
            dataGridViewMissingDates.ReadOnly = true;
            dataGridViewMissingDates.RowHeadersVisible = false;
        }

        private void PopulateMissingDatesGrid(List<string> missingDates)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => PopulateMissingDatesGrid(missingDates)));
                return;
            }

            // Clear existing rows
            dataGridViewMissingDates.Rows.Clear();

            // Add missing dates as rows
            foreach (var dateRange in missingDates)
            {
                dataGridViewMissingDates.Rows.Add(dateRange);
            }
        }

        private async Task CheckCallRecordingGapsOptimized()
        {
            string rootFolder = @"\\marketingnas\Phone System\Recordings";

            try
            {
                var subfolders = Directory.GetDirectories(rootFolder)
                    .Select(Path.GetFileName)
                    .Where(name => int.TryParse(name, out _))
                    .OrderByDescending(name => name)
                    .Take(2)
                    .ToList();

                var missingDates = new List<string>();

                foreach (var folder in subfolders)
                {
                    string folderPath = Path.Combine(rootFolder, folder);

                    // Stream file modified dates
                    var modifiedDates = await Task.Run(() => GetModifiedDates(folderPath));

                    // Find and store missing date ranges
                    missingDates.AddRange(FindMissingDateRanges(modifiedDates));
                }

                PopulateMissingDatesGrid(missingDates);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Access denied to the folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show($"Folder not found: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private IEnumerable<DateTime> GetModifiedDates(string folderPath)
        {
            HashSet<DateTime> dateSet = new HashSet<DateTime>();

            foreach (var filePath in Directory.EnumerateFiles(folderPath))
            {
                DateTime modifiedDate = File.GetLastWriteTime(filePath).Date;

                // Add the date to the set (avoids duplicates)
                dateSet.Add(modifiedDate);
            }

            return dateSet.OrderBy(date => date);
        }

        private List<string> FindMissingDateRanges(IEnumerable<DateTime> modifiedDates)
        {
            var missingRanges = new List<string>();

            if (!modifiedDates.Any()) return missingRanges;

            DateTime current = modifiedDates.First();
            DateTime end = modifiedDates.Last();

            while (current <= end)
            {
                if (!modifiedDates.Contains(current))
                {
                    DateTime missingStart = current;

                    // Find the end of the missing range
                    while (current <= end && !modifiedDates.Contains(current))
                    {
                        current = current.AddDays(1);
                    }

                    DateTime missingEnd = current.AddDays(-1);

                    if (missingStart == missingEnd)
                    {
                        missingRanges.Add($"{missingStart:yyyy-MM-dd}");
                    }
                    else
                    {
                        missingRanges.Add($"{missingStart:yyyy-MM-dd} to {missingEnd:yyyy-MM-dd}");
                    }
                }

                current = current.AddDays(1);
            }

            return missingRanges;
        }
        

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
        }
    }
}
