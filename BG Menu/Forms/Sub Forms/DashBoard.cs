using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using BG_Menu.Data;
using BG_Menu.Class.Sales_Summary;


namespace BG_Menu.Forms.Sub_Forms
{
    public partial class DashBoard : Form
    {
        private SalesRepository salesRepository;

        private int currentLogIndex = -1;

        public DashBoard()
        {
            InitializeComponent();
            salesRepository = GlobalInstances.SalesRepository;
        }

        private async void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadFaultsAsync();
                UpdateListViewData();
            }
            catch (Exception ex)
            {

            }
        }

        #region Faults + Servers               

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
