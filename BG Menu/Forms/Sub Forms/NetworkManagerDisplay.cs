using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Timer = System.Threading.Timer;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data.SqlClient;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class NetworkManagerDisplay : Form
    {
        private const int TcpPort = 50550;
        private TcpListener tcpListener;
        private CancellationTokenSource cancellationTokenSource;
        private Timer checkTimer;
        private SortableBindingList<MachineData> machineDataList = new SortableBindingList<MachineData>();

        // ISP Highlight Settings
        private static readonly List<string> FranchiseStoreISPs = new List<string> { "Plan Communications Limited", "Vodafone Limited" };
        private static readonly List<string> UKStoreISPs = new List<string> { "British Telecommunications PLC", "Vodafone Limited" };

        // CheckBoxes for filtering
        //private CheckBox checkBoxTill;
        //private CheckBox checkBoxTablet;
        //private CheckBox checkBoxLaptop;
        // checkBoxPreserveScroll already exists and is initialized elsewhere

        public NetworkManagerDisplay()
        {
            InitializeComponent();
            InitializeDataGridView();            
            SetDoubleBuffering(dataGridView1, true);
            PrePopulateDataGridView();
            StartListening();
            InitializeCheckTimer();

            checkBoxTill.CheckedChanged += CheckBoxFilter_CheckedChanged;
            checkBoxTablet.CheckedChanged += CheckBoxFilter_CheckedChanged;
            checkBoxLaptop.CheckedChanged += CheckBoxFilter_CheckedChanged;

            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Clear();

            // Define columns with SortMode set to NotSortable
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MachineName",
                HeaderText = "Machine Name",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Location",
                HeaderText = "Location",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "WANIP",
                HeaderText = "WAN IP",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ISP",
                HeaderText = "ISP",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CPUInfo",
                HeaderText = "CPU Info",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RAMInfo",
                HeaderText = "RAM Info",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StorageInfo",
                HeaderText = "Storage Info",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "WindowsOS",
                HeaderText = "Windows OS",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BuildNumber",
                HeaderText = "Build Number",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DateTimeReceived",
                HeaderText = "Date and Time Received",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SenderVersion",
                HeaderText = "Sender Version",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PendingUpdates",
                HeaderText = "Pending Updates",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LatestSharepointFile",
                HeaderText = "Latest SharePoint File",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd-MM-yyyy" } // Format the date as desired
            });


            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font.FontFamily, 12, FontStyle.Bold);

            dataGridView1.DataSource = machineDataList;
            dataGridView1.RowPrePaint += DataGridView1_RowPrePaint;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }        

        private void CheckBoxFilter_CheckedChanged(object sender, EventArgs e)
        {
            ApplyRowFilter();
        }

        private void SetDoubleBuffering(Control control, bool enabled)
        {
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { enabled });
        }

        private void PrePopulateDataGridView()
        {
            foreach (var entry in StoreLocationManager.StoreLocations)
            {
                var machineData = new MachineData
                {
                    MachineName = entry.Key,
                    Location = entry.Value.Location,
                    DateTimeReceived = null, // Leave as null
                    WANIP = "-",
                    ISP = "-",
                    CPUInfo = "-",
                    RAMInfo = "-",
                    StorageInfo = "-",
                    WindowsOS = "-",
                    BuildNumber = "-",
                    SenderVersion = "-",                    
                };
                SetRowColor(machineData); // Set initial row color
                machineDataList.Add(machineData);
            }
            ApplyRowFilter(); // Apply the filter after populating
        }

        private async void StartListening()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, TcpPort);
                tcpListener.Start();
                cancellationTokenSource = new CancellationTokenSource();

                await Task.Run(async () =>
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        TcpClient client = await tcpListener.AcceptTcpClientAsync();
                        _ = Task.Run(() => HandleClientAsync(client, cancellationTokenSource.Token), cancellationTokenSource.Token);
                    }
                }, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting TCP listener: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[4096]; // Adjust buffer size if needed
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    UpdateMachineData(receivedData);
                }
            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    MessageBox.Show($"Error handling client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateMachineData(string data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateMachineData(data)));
                return;
            }

            try
            {
                // Assuming the data format is (MainData)|MachineName|WANIP|ISP|CPUInfo|RAMInfo|StorageInfo|WindowsOS|BuildNumber|SenderVersion|PendingUpdates|LatestSharepointFile
                string[] parts = data.Split('|');
                if (parts.Length < 12)
                {
                    return;
                }

                string machineName = parts[1];
                string wanIp = parts[2];
                string isp = parts[3];
                string cpuInfo = parts[4];
                string ramInfo = parts[5];
                string storageInfo = parts[6];
                string windowsOS = parts[7];
                string buildNumber = parts[8];
                string senderVersion = parts[9];
                string pendingUpdates = parts[10];
                DateTime? latestSharepointFile = DateTime.TryParse(parts[11], out DateTime parsedDate) ? parsedDate : (DateTime?)null;
                DateTime dateTimeReceived = DateTime.Now;

                var machine = machineDataList.FirstOrDefault(m => m.MachineName == machineName);
                if (machine != null)
                {
                    // Update existing machine data
                    machine.WANIP = wanIp;
                    machine.ISP = isp;
                    machine.CPUInfo = cpuInfo;
                    machine.RAMInfo = ramInfo;
                    machine.StorageInfo = storageInfo;
                    machine.WindowsOS = windowsOS;
                    machine.BuildNumber = buildNumber;
                    machine.SenderVersion = senderVersion;
                    machine.DateTimeReceived = dateTimeReceived;
                    machine.PendingUpdates = pendingUpdates;
                    machine.LatestSharepointFile = latestSharepointFile;

                    SetRowColor(machine);
                }
                else
                {
                    // Add new machine data
                    var newMachine = new MachineData
                    {
                        MachineName = machineName,
                        Location = StoreLocationManager.GetLocationByMachineName(machineName),
                        WANIP = wanIp,
                        ISP = isp,
                        CPUInfo = cpuInfo,
                        RAMInfo = ramInfo,
                        StorageInfo = storageInfo,
                        WindowsOS = windowsOS,
                        BuildNumber = buildNumber,
                        SenderVersion = senderVersion,
                        DateTimeReceived = dateTimeReceived,
                        PendingUpdates = pendingUpdates,
                        LatestSharepointFile = latestSharepointFile
                    };

                    SetRowColor(newMachine);
                    machineDataList.Add(newMachine);
                }

                ApplyRowFilter(); // Apply the filter after updating data

                SendDataToSqlDatabase(machineName, machine.Location, wanIp, isp, cpuInfo, ramInfo, storageInfo, windowsOS, buildNumber, senderVersion, pendingUpdates, latestSharepointFile, dateTimeReceived);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating machine data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetRowColor(MachineData machine)
        {
            if (machine.DateTimeReceived.HasValue)
            {
                if ((DateTime.Now - machine.DateTimeReceived.Value).TotalSeconds > 130)
                {
                    machine.RowColor = Color.Tomato;
                    machine.SortOrder = 1;
                }
                else if (!string.IsNullOrEmpty(machine.MachineName) && !string.IsNullOrEmpty(machine.ISP))
                {
                    if (StoreLocationManager.StoreLocations.ContainsKey(machine.MachineName))
                    {
                        var storeType = StoreLocationManager.GetStoreTypeByMachineName(machine.MachineName);

                        if (storeType == "Franchise Store" && FranchiseStoreISPs.Contains(machine.ISP))
                        {
                            machine.RowColor = Color.Yellow;
                            machine.SortOrder = 2;
                        }
                        else if (storeType == "UK Store" && UKStoreISPs.Contains(machine.ISP))
                        {
                            machine.RowColor = Color.Yellow;
                            machine.SortOrder = 2;
                        }
                        else
                        {
                            machine.RowColor = Color.LightGreen;
                            machine.SortOrder = 3;
                        }
                    }
                    else
                    {
                        machine.RowColor = Color.LightGreen;
                        machine.SortOrder = 3;
                    }
                }
                else
                {
                    machine.RowColor = Color.White;
                    machine.SortOrder = 4;
                }
            }
            else
            {
                // If DateTimeReceived is null, leave row color as default (white)
                machine.RowColor = Color.White;
                machine.SortOrder = 4;
            }
        }

        private void InitializeCheckTimer()
        {
            checkTimer = new Timer(CheckForInactiveMachines, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
        }

        private void CheckForInactiveMachines(object state)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CheckForInactiveMachines(null)));
                return;
            }

            foreach (var machine in machineDataList)
            {
                SetRowColor(machine);
            }

            // Check the state of the checkbox
            bool preserveScrollPosition = checkBoxPreserveScroll.Checked;

            int firstDisplayedRowIndex = -1;
            MachineData firstDisplayedMachine = null;
            int? selectedRowIndex = null;
            int? selectedColumnIndex = null;

            if (preserveScrollPosition)
            {
                // Store the current scroll position and the first displayed machine
                firstDisplayedRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;

                // Store the currently selected cell (optional)
                if (dataGridView1.CurrentCell != null)
                {
                    selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                    selectedColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
                }

                // Store the machine data of the first displayed row
                if (firstDisplayedRowIndex >= 0 && firstDisplayedRowIndex < machineDataList.Count)
                {
                    firstDisplayedMachine = machineDataList[firstDisplayedRowIndex];
                }
            }

            // Sort the list with multiple criteria
            var sortProperties = new List<(PropertyDescriptor Property, ListSortDirection Direction)>
            {
                (TypeDescriptor.GetProperties(typeof(MachineData))["SortOrder"], ListSortDirection.Ascending),
                (TypeDescriptor.GetProperties(typeof(MachineData))["Location"], ListSortDirection.Ascending),
                (TypeDescriptor.GetProperties(typeof(MachineData))["MachineName"], ListSortDirection.Ascending)
            };

            machineDataList.ApplySort(sortProperties);

            if (preserveScrollPosition)
            {
                // Restore the scroll position
                if (firstDisplayedMachine != null)
                {
                    int newIndex = machineDataList.IndexOf(firstDisplayedMachine);
                    if (newIndex >= 0 && newIndex < dataGridView1.Rows.Count)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = newIndex;
                    }
                }
                else if (firstDisplayedRowIndex >= 0 && firstDisplayedRowIndex < dataGridView1.Rows.Count)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = firstDisplayedRowIndex;
                }

                // Restore the selected cell (optional)
                if (selectedRowIndex.HasValue && selectedColumnIndex.HasValue)
                {
                    if (selectedRowIndex.Value >= 0 && selectedRowIndex.Value < dataGridView1.Rows.Count &&
                        selectedColumnIndex.Value >= 0 && selectedColumnIndex.Value < dataGridView1.Columns.Count)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[selectedRowIndex.Value].Cells[selectedColumnIndex.Value];
                    }
                }
            }
            else
            {
                // Scroll to the top of the DataGridView
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                }
            }

            ApplyRowFilter(); // Apply the filter after sorting
        }

        private void ApplyRowFilter()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyRowFilter));
                return;
            }

            CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            int currentPosition = currencyManager.Position;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DataBoundItem is MachineData machine)
                {
                    bool isVisible = true;
                    string locationLower = machine.Location?.ToLower() ?? "";

                    if (!checkBoxTill.Checked && locationLower.Contains("till"))
                    {
                        isVisible = false;
                    }
                    if (!checkBoxTablet.Checked && locationLower.Contains("queuebuster"))
                    {
                        isVisible = false;
                    }
                    if (!checkBoxLaptop.Checked && locationLower.Contains("workshop"))
                    {
                        isVisible = false;
                    }

                    // Check if the row is the current row
                    if (!isVisible && row.Index == dataGridView1.CurrentCell?.RowIndex)
                    {
                        // Move the current cell to a visible row
                        DataGridViewRow newCurrentRow = dataGridView1.Rows
                            .Cast<DataGridViewRow>()
                            .FirstOrDefault(r => r.Visible && r.Index != row.Index);

                        if (newCurrentRow != null)
                        {
                            dataGridView1.CurrentCell = newCurrentRow.Cells[0];
                        }
                        else
                        {
                            // If no other visible rows are available, clear the current cell
                            dataGridView1.CurrentCell = null;
                        }
                    }

                    // Set the visibility of the row
                    row.Visible = isVisible;
                }
            }
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var machine = machineDataList[e.RowIndex];
            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = machine.RowColor;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
            var machine = machineDataList[e.RowIndex];

            if (columnName == "DateTimeReceived")
            {
                if (!machine.DateTimeReceived.HasValue)
                {
                    e.Value = "-"; // Display "-" if DateTimeReceived is null
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = machine.DateTimeReceived.Value.ToString();
                    e.FormattingApplied = true;
                }
            }
            else if (columnName == "StorageInfo")
            {
                try
                {
                    var storageInfo = machine.StorageInfo;
                    if (!string.IsNullOrEmpty(storageInfo))
                    {
                        // Remove "GB" and any trailing or leading spaces
                        var storageInfoWithoutGB = storageInfo.Replace("GB", "").Trim();

                        // Split the string into used and total storage
                        var parts = storageInfoWithoutGB.Split('/');
                        if (parts.Length == 2)
                        {
                            // Try to parse the used and total storage values
                            if (double.TryParse(parts[0], out double usedSpace) && double.TryParse(parts[1], out double totalSpace))
                            {
                                if (totalSpace > 0)
                                {
                                    // Calculate the percentage of used storage
                                    double percentUsed = (usedSpace / totalSpace) * 100;
                                    if (percentUsed >= 85)
                                    {
                                        // Set the cell's background color to Tomato if usage is >= 85%
                                        e.CellStyle.BackColor = Color.Tomato;
                                    }
                                    else
                                    {
                                        // Set the cell's background color to match the row's color
                                        e.CellStyle.BackColor = machine.RowColor;
                                    }
                                }
                                else
                                {
                                    // Handle division by zero by setting cell background to row color
                                    e.CellStyle.BackColor = machine.RowColor;
                                }
                            }
                            else
                            {
                                // If parsing fails, set the cell's background color to row color
                                e.CellStyle.BackColor = machine.RowColor;
                            }
                        }
                        else
                        {
                            // If splitting fails, set the cell's background color to row color
                            e.CellStyle.BackColor = machine.RowColor;
                        }
                    }
                    else
                    {
                        // If storageInfo is empty, set the cell's background color to row color
                        e.CellStyle.BackColor = machine.RowColor;
                    }
                    e.FormattingApplied = true;
                }
                catch
                {
                    // In case of any exception, set the cell's background color to row color
                    e.CellStyle.BackColor = machine.RowColor;
                    e.FormattingApplied = true;
                }
            }

            else if (columnName == "PendingUpdates")
            {
                // Refined check for PendingUpdates to handle variations of "None" with spaces or different cases
                string pendingUpdatesValue = machine.PendingUpdates?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(pendingUpdatesValue) || pendingUpdatesValue.Equals("None", StringComparison.OrdinalIgnoreCase))
                {
                    // If "None" or empty, display "No" with the default background color (row color)
                    e.Value = "No";
                    e.CellStyle.BackColor = machine.RowColor;
                    dataGridView1[e.ColumnIndex, e.RowIndex].ToolTipText = string.Empty; // Clear any existing tooltip
                }
                else
                {
                    // If there's actual update information, display "Yes" with a Tomato background color
                    e.Value = "Yes";
                    e.CellStyle.BackColor = Color.Tomato;

                    // Assign a tooltip for cells with pending updates
                    string tooltipText = machine.PendingUpdates;
                    dataGridView1[e.ColumnIndex, e.RowIndex].ToolTipText = tooltipText;
                }

                e.FormattingApplied = true;
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is within valid bounds
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the clicked cell and corresponding MachineData object
                var columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
                if (columnName == "PendingUpdates")
                {
                    var machine = machineDataList[e.RowIndex];
                    if (!string.IsNullOrEmpty(machine.PendingUpdates) && machine.PendingUpdates != "None")
                    {
                        // Show the pending updates in a message box
                        MessageBox.Show(machine.PendingUpdates, "Pending Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // No updates available
                        MessageBox.Show("No pending updates.", "Pending Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private async void SendDataToFileServer(MachineData machine)
        {
            try
            {
                string fileServerUrl = "http://bananagoats.co.uk:50547/receiveData"; // Adjust the URL and port if necessary

                using (var httpClient = new HttpClient())
                {
                    var jsonData = JsonConvert.SerializeObject(machine);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(fileServerUrl, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error sending data to FileServer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendDataToSqlDatabase(string machineName, string location, string wanIp, string isp, string cpuInfo, string ramInfo, string storageInfo, string windowsOS, string buildNumber, string senderVersion, string pendingUpdates, DateTime? latestSharepointFile, DateTime dateTimeReceived)
        {
            // Replace with your actual connection string
            string connectionString = "Server=Bananagoats.co.uk;Database=Ableworld;User Id=Elliot;Password=1234;";

            string query = @"
        MERGE INTO MachineData AS target
        USING (SELECT @MachineName AS MachineName) AS source
        ON target.MachineName = source.MachineName
        WHEN MATCHED THEN
            UPDATE SET
                Location = @Location,
                WANIP = @WANIP,
                ISP = @ISP,
                CPUInfo = @CPUInfo,
                RAMInfo = @RAMInfo,
                StorageInfo = @StorageInfo,
                WindowsOS = @WindowsOS,
                BuildNumber = @BuildNumber,
                SenderVersion = @SenderVersion,
                PendingUpdates = @PendingUpdates,
                LatestSharepointFile = @LatestSharepointFile,
                DateTimeReceived = @DateTimeReceived
        WHEN NOT MATCHED THEN
            INSERT (MachineName, Location, WANIP, ISP, CPUInfo, RAMInfo, StorageInfo, WindowsOS, BuildNumber, SenderVersion, PendingUpdates, LatestSharepointFile, DateTimeReceived)
            VALUES (@MachineName, @Location, @WANIP, @ISP, @CPUInfo, @RAMInfo, @StorageInfo, @WindowsOS, @BuildNumber, @SenderVersion, @PendingUpdates, @LatestSharepointFile, @DateTimeReceived);";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@MachineName", machineName);
                    command.Parameters.AddWithValue("@Location", location ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@WANIP", wanIp ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ISP", isp ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CPUInfo", cpuInfo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RAMInfo", ramInfo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StorageInfo", storageInfo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@WindowsOS", windowsOS ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BuildNumber", buildNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SenderVersion", senderVersion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PendingUpdates", pendingUpdates ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LatestSharepointFile", latestSharepointFile ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateTimeReceived", dateTimeReceived);

                    // Open the connection and execute the query
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending data to SQL database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            tcpListener?.Stop();
            checkTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }

    // Data model for machine data
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
        public DateTime? DateTimeReceived { get; set; } // Nullable DateTime
        public string SenderVersion { get; set; }
        [Browsable(false)] // Hide RowColor from DataGridView columns
        public Color RowColor { get; set; }
        [Browsable(false)] // Hide SortOrder from DataGridView columns
        public int SortOrder { get; set; }

        public string PendingUpdates { get; set; }
        public DateTime? LatestSharepointFile { get; set; }
    }

    // Updated SortableBindingList<T> class
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSortedValue;
        private List<(PropertyDescriptor Property, ListSortDirection Direction)> sortProperties;

        protected override bool SupportsSortingCore => true;

        public void ApplySort(List<(PropertyDescriptor Property, ListSortDirection Direction)> sortProperties)
        {
            this.sortProperties = sortProperties;

            var itemsList = Items as List<T>;
            if (itemsList == null)
                return;

            itemsList.Sort(Compare);

            isSortedValue = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private int Compare(T x, T y)
        {
            foreach (var sortProperty in sortProperties)
            {
                var prop = sortProperty.Property;
                var direction = sortProperty.Direction;

                var xValue = prop.GetValue(x);
                var yValue = prop.GetValue(y);

                int result;

                if (xValue == null)
                    result = yValue == null ? 0 : -1;
                else if (yValue == null)
                    result = 1;
                else if (xValue is IComparable comparable)
                    result = comparable.CompareTo(yValue);
                else
                    result = xValue.Equals(yValue) ? 0 : xValue.ToString().CompareTo(yValue.ToString());

                if (direction == ListSortDirection.Descending)
                    result = -result;

                if (result != 0)
                    return result;
            }

            return 0;
        }

        protected override void RemoveSortCore()
        {
            isSortedValue = false;
        }

        protected override bool IsSortedCore => isSortedValue;
    }
}
