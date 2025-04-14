using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Newtonsoft.Json;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class NetworkManagerDisplay : Form
    {
        private System.Windows.Forms.Timer checkTimer;

        private OfflineForm offlineForm;

        private SortableBindingList<MachineData> machineDataList = new SortableBindingList<MachineData>();

        private static readonly List<string> FranchiseStoreISPs = new List<string> { "Plan Communications Limited", "Vodafone Limited" };
        private static readonly List<string> UKStoreISPs = new List<string> { "British Telecommunications PLC", "Vodafone Limited" };

        public NetworkManagerDisplay()
        {
            InitializeComponent();
            InitializeDataGridView();

            LoadDataFromDatabase();

            InitializeCheckTimer();

            checkBoxTill.CheckedChanged += CheckBoxFilter_CheckedChanged;
            checkBoxTablet.CheckedChanged += CheckBoxFilter_CheckedChanged;
            checkBoxLaptop.CheckedChanged += CheckBoxFilter_CheckedChanged;
            checkBoxStairlifts.CheckedChanged += CheckBoxFilter_CheckedChanged;
            checkBoxTest.CheckedChanged += CheckBoxFilter_CheckedChanged;

            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        #region Data Loading from SQL

        private void LoadDataFromDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            string query = @"
        SELECT 
             [Name]
            ,[Store]
            ,[CPU]
            ,[Ram]
            ,[HHD]
            ,[IP]
            ,[ISP]
            ,[OS]
            ,[OS_Version]
            ,[Client_Version]
            ,[OS_Updates]
            ,[Sharepoint_Sync]
            ,[Pulse_Time]
            ,[CommsVersion]
            ,[TillVersion]
        FROM [dbo].[TBPC]";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Clear out any existing data before re-populating
                        machineDataList.Clear();

                        while (reader.Read())
                        {
                            var machine = new MachineData
                            {
                                MachineName = reader["Name"]?.ToString(),
                                Location = reader["Store"]?.ToString(),
                                CPUInfo = reader["CPU"]?.ToString(),
                                RAMInfo = reader["Ram"]?.ToString(),
                                StorageInfo = reader["HHD"]?.ToString(),
                                WANIP = reader["IP"]?.ToString(),
                                ISP = reader["ISP"]?.ToString(),
                                WindowsOS = reader["OS"]?.ToString(),
                                BuildNumber = reader["OS_Version"]?.ToString(),
                                SenderVersion = reader["Client_Version"]?.ToString(),
                                PendingUpdates = reader["OS_Updates"]?.ToString(),

                                DateTimeReceived = reader["Pulse_Time"] is DBNull
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["Pulse_Time"]),

                                LatestSharepointFile = reader["Sharepoint_Sync"] is DBNull
                                    ? "No data"
                                    : reader["Sharepoint_Sync"].ToString(),

                                CommsVersion = reader["CommsVersion"] is DBNull
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["CommsVersion"]),

                                TillVersion = reader["TillVersion"] is DBNull
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["TillVersion"]),
                            };

                            SetRowColor(machine);
                            machineDataList.Add(machine);
                        }
                    }
                }

                ApplyRowFilter();

                // If we reach here, the SQL connection succeeded.
                // Close the offline window if it's open.
                if (offlineForm != null && !offlineForm.IsDisposed)
                {
                    offlineForm.Close();
                    offlineForm = null;
                }
            }
            catch (Exception ex)
            {
                // Optionally log ex.Message if needed

                // If the offline window is not already open, create and show it.
                if (offlineForm == null || offlineForm.IsDisposed)
                {
                    offlineForm = new OfflineForm();
                    offlineForm.Show();
                }
            }
        }


        #endregion

        #region DataGridView Setup

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            // Define columns with SortMode = NotSortable
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MachineName",
                HeaderText = "Machine Name",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Location",
                HeaderText = "Store",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "WANIP",
                Name = "WANIP",
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
                Name = "CPUInfo",
                HeaderText = "CPU Info",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RAMInfo",
                Name = "RAMInfo",
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
                HeaderText = "Pulse Time",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SenderVersion",
                Name = "ClientVersion",
                HeaderText = "Client Version",
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
                HeaderText = "SharePoint Sync",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CommsVersion",
                HeaderText = "Comms Version",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TillVersion",
                HeaderText = "Till Version",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // Hide desired columns by default
            dataGridView1.Columns["WANIP"].Visible = false;
            dataGridView1.Columns["CPUInfo"].Visible = false;
            dataGridView1.Columns["RAMInfo"].Visible = false;
            dataGridView1.Columns["ClientVersion"].Visible = false;

            // Common style settings
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font.FontFamily, 12, FontStyle.Bold);
            dataGridView1.DataSource = machineDataList;

            // Event handlers for row color and cell formatting
            dataGridView1.RowPrePaint += DataGridView1_RowPrePaint;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;

            // Optionally, enable double-buffering to reduce flicker
            SetDoubleBuffering(dataGridView1, true);
        }        

        private void SetDoubleBuffering(Control control, bool enabled)
        {
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                control,
                new object[] { enabled }
            );
        }

        #endregion

        #region Row Coloring & Filtering Logic

        private void SetRowColor(MachineData machine)
        {
            if (machine.DateTimeReceived.HasValue)
            {
                // If last pulse was more than 130 seconds ago => Red
                if ((DateTime.Now - machine.DateTimeReceived.Value).TotalSeconds > 130)
                {
                    machine.RowColor = Color.Tomato;
                    machine.SortOrder = 1;
                }
                else if (!string.IsNullOrEmpty(machine.MachineName) && !string.IsNullOrEmpty(machine.ISP))
                {
                    if (machine.Location.Equals("Franchise Store", StringComparison.OrdinalIgnoreCase)
                        && FranchiseStoreISPs.Contains(machine.ISP))
                    {
                        machine.RowColor = Color.Yellow;
                        machine.SortOrder = 2;
                    }
                    else if (machine.Location.Equals("UK Store", StringComparison.OrdinalIgnoreCase)
                             && UKStoreISPs.Contains(machine.ISP))
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
                    machine.RowColor = Color.White;
                    machine.SortOrder = 4;
                }
            }
            else
            {
                // If Pulse_Time is null, default to white
                machine.RowColor = Color.White;
                machine.SortOrder = 4;
            }
        }

        private void CheckBoxFilter_CheckedChanged(object sender, EventArgs e)
        {
            ApplyRowFilter();
        }

        private void ApplyRowFilter()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyRowFilter));
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DataBoundItem is MachineData machine)
                {
                    bool isVisible = true;
                    string locationLower = machine.Location?.ToLower() ?? "";

                    if (!checkBoxTill.Checked && locationLower.Contains("till"))
                        isVisible = false;
                    if (!checkBoxTablet.Checked && locationLower.Contains("queuebuster"))
                        isVisible = false;
                    if (!checkBoxLaptop.Checked && locationLower.Contains("workshop"))
                        isVisible = false;
                    if (!checkBoxStairlifts.Checked && locationLower.Contains("stairlift"))
                        isVisible = false;
                    if (!checkBoxTest.Checked && locationLower.Contains("test"))
                        isVisible = false;
                    if (!checkBoxTest.Checked && locationLower.Contains("office"))
                        isVisible = false;

                    // If the row is currently selected but is about to be hidden, move selection
                    if (!isVisible && row.Index == dataGridView1.CurrentCell?.RowIndex)
                    {
                        var newCurrentRow = dataGridView1.Rows
                            .Cast<DataGridViewRow>()
                            .FirstOrDefault(r => r.Visible && r.Index != row.Index);

                        dataGridView1.CurrentCell = newCurrentRow?.Cells[0];
                    }

                    row.Visible = isVisible;
                }
            }
        }

        #endregion

        #region WinForms Timer & Sorting

        private void InitializeCheckTimer()
        {
            // Replaces System.Threading.Timer with a WinForms Timer
            checkTimer = new System.Windows.Forms.Timer();
            checkTimer.Interval = 2000; // e.g. 2 seconds
            checkTimer.Tick += (s, e) => CheckForInactiveMachines();
            checkTimer.Start();
        }

        // No more "object state" parameter needed here
        private void CheckForInactiveMachines()
        {
            // 1) Capture old scroll & selection *before* loading new data
            bool preserveScrollPosition = checkBoxPreserveScroll.Checked;

            int oldFirstDisplayedIndex = -1;
            MachineData oldFirstDisplayedMachine = null;
            int? oldSelectedRowIndex = null;
            int? oldSelectedColumnIndex = null;

            if (preserveScrollPosition)
            {
                // If the grid has any rows at all
                if (dataGridView1.Rows.Count > 0 && dataGridView1.FirstDisplayedScrollingRowIndex >= 0)
                {
                    oldFirstDisplayedIndex = dataGridView1.FirstDisplayedScrollingRowIndex;

                    // Identify the "machine" object currently at the top
                    if (oldFirstDisplayedIndex < dataGridView1.Rows.Count)
                    {
                        var topRowMachine = dataGridView1.Rows[oldFirstDisplayedIndex].DataBoundItem as MachineData;
                        oldFirstDisplayedMachine = topRowMachine;
                    }
                }

                if (dataGridView1.CurrentCell != null)
                {
                    oldSelectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                    oldSelectedColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
                }
            }

            // 2) Now load/reload data
            LoadDataFromDatabase();
            // (Remove the ApplyRowFilter() call inside LoadDataFromDatabase() if necessary.)

            // 3) Re-apply color
            foreach (var machine in machineDataList)
                SetRowColor(machine);

            // 4) Sort
                    var sortProperties = new List<(PropertyDescriptor Property, ListSortDirection Direction)>
            {
                (TypeDescriptor.GetProperties(typeof(MachineData))["SortOrder"], ListSortDirection.Ascending),
                (TypeDescriptor.GetProperties(typeof(MachineData))["Location"],  ListSortDirection.Ascending),
                (TypeDescriptor.GetProperties(typeof(MachineData))["MachineName"], ListSortDirection.Ascending)
            };
            machineDataList.ApplySort(sortProperties);

            // 5) Apply filter *before* we restore scroll position
            ApplyRowFilter();

            // 6) Restore scroll/selection
            if (preserveScrollPosition)
            {
                // Try to scroll to the same machine that was at the top before
                if (oldFirstDisplayedMachine != null)
                {
                    int newIndex = machineDataList.IndexOf(oldFirstDisplayedMachine);
                    if (newIndex >= 0 && newIndex < dataGridView1.Rows.Count)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = newIndex;
                    }
                }
                // Otherwise fall back to the oldFirstDisplayedIndex, if still valid
                else if (oldFirstDisplayedIndex >= 0 && oldFirstDisplayedIndex < dataGridView1.Rows.Count)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = oldFirstDisplayedIndex;
                }

                // Restore selection if still valid & the row is visible
                if (oldSelectedRowIndex.HasValue && oldSelectedColumnIndex.HasValue)
                {
                    int sr = oldSelectedRowIndex.Value;
                    int sc = oldSelectedColumnIndex.Value;
                    if (sr >= 0 && sr < dataGridView1.Rows.Count &&
                        sc >= 0 && sc < dataGridView1.Columns.Count &&
                        dataGridView1.Rows[sr].Visible)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[sr].Cells[sc];
                    }
                }
            }
        }


        #endregion

        #region DataGridView Events

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var machine = machineDataList[e.RowIndex];
            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = machine.RowColor;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
            var machine = machineDataList[e.RowIndex];

            // Pulse time display
            if (columnName == "DateTimeReceived")
            {
                if (!machine.DateTimeReceived.HasValue)
                {
                    e.Value = "-";
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = machine.DateTimeReceived.Value.ToString("yyyy-MM-dd HH:mm:ss");
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
                        // e.g. "120GB / 200GB"
                        var cleaned = storageInfo.Replace("GB", "").Trim();
                        var parts = cleaned.Split('/');
                        if (parts.Length == 2 &&
                            double.TryParse(parts[0], out double usedSpace) &&
                            double.TryParse(parts[1], out double totalSpace) &&
                            totalSpace > 0)
                        {
                            double percentUsed = (usedSpace / totalSpace) * 100;
                            if (percentUsed >= 85)
                            {
                                e.CellStyle.BackColor = Color.Tomato;
                            }
                            else
                            {
                                e.CellStyle.BackColor = machine.RowColor;
                            }
                        }
                        else
                        {
                            e.CellStyle.BackColor = machine.RowColor;
                        }
                    }
                    else
                    {
                        e.CellStyle.BackColor = machine.RowColor;
                    }
                    e.FormattingApplied = true;
                }
                catch
                {
                    e.CellStyle.BackColor = machine.RowColor;
                    e.FormattingApplied = true;
                }
            }
            else if (columnName == "PendingUpdates")
            {
                string pendingUpdatesValue = machine.PendingUpdates?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(pendingUpdatesValue) ||
                    pendingUpdatesValue.Equals("No", StringComparison.OrdinalIgnoreCase))
                {
                    e.Value = "No";
                    e.CellStyle.BackColor = machine.RowColor;
                    dataGridView1[e.ColumnIndex, e.RowIndex].ToolTipText = string.Empty;
                }
                else
                {
                    e.Value = "Yes";
                    e.CellStyle.BackColor = Color.Tomato;
                    dataGridView1[e.ColumnIndex, e.RowIndex].ToolTipText = pendingUpdatesValue;
                }
                e.FormattingApplied = true;
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Example: If user clicks on "PendingUpdates", show the full list in a MessageBox
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
                if (columnName == "PendingUpdates")
                {
                    var machine = machineDataList[e.RowIndex];
                    if (!string.IsNullOrEmpty(machine.PendingUpdates) &&
                        !machine.PendingUpdates.Equals("None", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(machine.PendingUpdates, "Pending Updates",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No pending updates.", "Pending Updates",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        #endregion

        #region Form Closing

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Dispose the WinForms timer
            checkTimer?.Dispose();
            base.OnFormClosing(e);
        }

        #endregion

        #region Reload Button Example

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
            MessageBox.Show("Data reloaded successfully from database.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }

    /// <summary>
    /// Data model for machine data
    /// </summary>
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
        public string LatestSharepointFile { get; set; }

        public DateTime? CommsVersion { get; set; }
        public DateTime? TillVersion { get; set; }

        [Browsable(false)]
        public Color RowColor { get; set; }

        [Browsable(false)]
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// An updated sortable binding list that can sort by multiple columns.
    /// </summary>
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSortedValue;
        private List<(PropertyDescriptor Property, ListSortDirection Direction)> sortProperties;

        protected override bool SupportsSortingCore => true;

        public void ApplySort(List<(PropertyDescriptor Property, ListSortDirection Direction)> sortProps)
        {
            sortProperties = sortProps;
            if (Items is List<T> list)
            {
                list.Sort(Compare);
                isSortedValue = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
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
                    result = (yValue == null) ? 0 : -1;
                else if (yValue == null)
                    result = 1;
                else if (xValue is IComparable comparable)
                    result = comparable.CompareTo(yValue);
                else
                    result = xValue.ToString().CompareTo(yValue.ToString());

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
