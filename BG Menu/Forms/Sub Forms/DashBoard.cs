using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using BG_Menu.Data;
using BG_Menu.Class.Sales_Summary;
using System.Windows.Forms;


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
            LoadFaultsAsync();

            SetupGrid();
            LoadUnknownEntries();
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

        #endregion

        #region Unknown Machines

        private void SetupGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            // hidden “Source” column (so we know which table to edit)
            var srcCol = new DataGridViewTextBoxColumn
            {
                Name = "Source",
                DataPropertyName = "Source",
                Visible = false
            };
            dataGridView1.Columns.Add(srcCol);

            // MachineName (or Machine) column
            var nameCol = new DataGridViewTextBoxColumn
            {
                Name = "MachineName",
                HeaderText = "Machine",
                DataPropertyName = "MachineName",
                ReadOnly = true
            };
            dataGridView1.Columns.Add(nameCol);

            // Edit button
            var btnCol = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "",
                Text = "Edit…",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(btnCol);

            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void LoadUnknownEntries()
        {
            dataGridView1.DataSource = GetUnknownEntries();
        }

        private DataTable GetUnknownEntries()
        {
            var dt = new DataTable();
            dt.Columns.Add("Source", typeof(string));
            dt.Columns.Add("MachineName", typeof(string));

            // 1) Computers where Location, Company or Type = 'Unknown' OR CompanyName IS NULL
            const string sqlComputers = @"
        SELECT MachineName
        FROM dbo.Computers
        WHERE 
            Location    = @unk
         OR Company     = @unk
         OR Type        = @unk
         OR CompanyName IS NULL;
    ";
            var p = new Dictionary<string, object> { { "@unk", "Unknown" } };
            var dtComp = salesRepository.ExecuteSqlQuery(sqlComputers, p);
            foreach (DataRow r in dtComp.Rows)
                dt.Rows.Add("Computers", r["MachineName"]);

            // 2) SalesSheetMapping as before...
            const string sqlMapping = @"
        SELECT Machine    AS MachineName
        FROM Sales.SalesSheetMapping
        WHERE Company = @unk
           OR Mapping = @unk;
    ";
            var dtMap = salesRepository.ExecuteSqlQuery(sqlMapping, p);
            foreach (DataRow r in dtMap.Rows)
                dt.Rows.Add("SalesSheetMapping", r["MachineName"]);

            return dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.Columns[e.ColumnIndex].Name != "Edit")
                return;

            var source = dataGridView1.Rows[e.RowIndex].Cells["Source"].Value.ToString();
            var machineName = dataGridView1.Rows[e.RowIndex].Cells["MachineName"].Value.ToString();

            Form editor;
            if (source == "Computers")
                editor = new EditComputerForm(machineName, salesRepository);
            else
                editor = new EditMappingForm(machineName, salesRepository);

            if (editor.ShowDialog(this) == DialogResult.OK)
                LoadUnknownEntries();
        }

        #endregion

    }
}
