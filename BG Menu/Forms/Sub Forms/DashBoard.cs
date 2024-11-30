using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
            InitializeTimer();

            // Ensure AutoGenerateColumns is false
            dataGridViewFaults.AutoGenerateColumns = false;
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            // No additional initialization needed here
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
    }
}
