using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using BG_Menu.Data;
using BG_Menu.Class.Sales_Summary;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class Updater : Form
    {
        private Timer _refreshTimer;
        private Timer _searchTimer;
        private SalesRepository salesRepository;

        public Updater()
        {
            InitializeComponent();

            salesRepository = GlobalInstances.SalesRepository;

            SetupDataGridViewColumns();

            LoadData();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 5000; // ...
            _refreshTimer.Tick += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    LoadData();
                else
                    LoadSearchData(txtSearch.Text);
            };
            _refreshTimer.Start();

            _searchTimer = new Timer();
            _searchTimer.Interval = 500;
            _searchTimer.Tick += _searchTimer_Tick;


            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Whenever text changes, restart (or start) the debounce timer
            // so we only search after user hasn't typed for a moment.
            _searchTimer.Stop(); // if it was running
            _searchTimer.Start();
        }

        private void _searchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadSearchData(txtSearch.Text);
            }
            else
            {
                LoadData();
            }
        }

        private void SetupDataGridViewColumns()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            // 1) "Name" (bound to MachineName)
            var colMachineName = new DataGridViewTextBoxColumn();
            colMachineName.Name = "MachineName";             // <-- Add this line
            colMachineName.HeaderText = "Name";
            colMachineName.DataPropertyName = "MachineName";
            dataGridView1.Columns.Add(colMachineName);

            // 2) "Location"
            var colLocation = new DataGridViewTextBoxColumn();
            colLocation.Name = "Location";                   // <-- Add this line
            colLocation.HeaderText = "Location";
            colLocation.DataPropertyName = "Location";
            dataGridView1.Columns.Add(colLocation);

            // 3) "Company"
            var colCompany = new DataGridViewTextBoxColumn();
            colCompany.Name = "CompanyName";                     // <-- Add this line
            colCompany.HeaderText = "Company";
            colCompany.DataPropertyName = "CompanyName";
            dataGridView1.Columns.Add(colCompany);

            // 4) "Type"
            var colType = new DataGridViewTextBoxColumn();
            colType.Name = "Type";                           // <-- Add this line
            colType.HeaderText = "Type";
            colType.DataPropertyName = "Type";
            dataGridView1.Columns.Add(colType);

            // 5) "Network Detector" - checkbox (AppUpdate)
            var colNetworkDetector = new DataGridViewCheckBoxColumn();
            colNetworkDetector.Name = "AppUpdate";           // <-- Add this line
            colNetworkDetector.HeaderText = "Network Detector";
            colNetworkDetector.DataPropertyName = "AppUpdate";
            colNetworkDetector.TrueValue = "Yes";
            colNetworkDetector.FalseValue = "No";
            dataGridView1.Columns.Add(colNetworkDetector);

            // 6) "Till Updater" - checkbox (TillUpdater)
            var colTillUpdater = new DataGridViewCheckBoxColumn();
            colTillUpdater.Name = "TillUpdater";             // <-- Add this line
            colTillUpdater.HeaderText = "Till Updater";
            colTillUpdater.DataPropertyName = "TillUpdater";
            colTillUpdater.TrueValue = "Yes";
            colTillUpdater.FalseValue = "No";
            dataGridView1.Columns.Add(colTillUpdater);

            // 7) "Call Pop Lite" - checkbox (CallPopLite)
            var colCallPopLite = new DataGridViewCheckBoxColumn();
            colCallPopLite.Name = "CallPopLite";             // <-- Add this line
            colCallPopLite.HeaderText = "Call Pop Lite";
            colCallPopLite.DataPropertyName = "CallPopLite";
            colCallPopLite.TrueValue = "Yes";
            colCallPopLite.FalseValue = "No";
            dataGridView1.Columns.Add(colCallPopLite);

            // 8) "FormRefresh"
            var colFormRefresh = new DataGridViewCheckBoxColumn();
            colFormRefresh.Name = "FormRefresh";             // <-- Add this line
            colFormRefresh.HeaderText = "Refresh Doc Templates";
            colFormRefresh.DataPropertyName = "FormRefresh";
            colFormRefresh.TrueValue = "Yes";
            colFormRefresh.FalseValue = "No";
            dataGridView1.Columns.Add(colFormRefresh);

            // 9) "Commands"
            var colCommands = new DataGridViewCheckBoxColumn();
            colCommands.Name = "Commands";
            colCommands.HeaderText = "Commands";
            colCommands.DataPropertyName = "Commands";
            colCommands.TrueValue = "Yes";
            colCommands.FalseValue = "No";
            dataGridView1.Columns.Add(colCommands);

            // 10) SpeedTest"
            var colSpeed = new DataGridViewCheckBoxColumn();
            colSpeed.Name = "Speed Test";
            colSpeed.HeaderText = "Speed Test";
            colSpeed.DataPropertyName = "SpeedTest";
            colSpeed.TrueValue = "Yes";
            colSpeed.FalseValue = "No";
            dataGridView1.Columns.Add(colSpeed);
        }


        private void LoadSearchData(string searchTerm)
        {
            int oldScrollIndex = -1;
            try
            {
                oldScrollIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            }
            catch { }

            string query = @"
                SELECT
                    MachineName,
                    Location,
                    CompanyName,
                    Type,
                    AppUpdate,
                    TillUpdater,
                    CallPopLite,
                    FormRefresh,
                    Commands,
                    SpeedTest
                FROM Computers
                WHERE 
                    (MachineName LIKE @search OR
                     Location    LIKE @search OR
                     CompanyName LIKE @search OR
                     Type        LIKE @search)";

            var parameters = new Dictionary<string, object>
            {
                { "@search", $"%{searchTerm}%" }
            };

            try
            {
                DataTable dt = salesRepository.ExecuteSqlQuery(query, parameters);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading search data: " + ex.Message);
            }

            if (oldScrollIndex >= 0 && oldScrollIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = oldScrollIndex;
            }
        }

        private void LoadData()
        {
            int oldScrollIndex = -1;
            try
            {
                oldScrollIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            }
            catch { }

            string query = @"
                SELECT
                    MachineName,
                    Location,
                    CompanyName,
                    Type,
                    AppUpdate,
                    TillUpdater,
                    CallPopLite,
                    FormRefresh,
                    Commands,
                    SpeedTest
                FROM Computers
                WHERE 
                    MachineName LIKE '%ABL%'
                    AND (
                        AppUpdate = 'Yes'
                        OR TillUpdater = 'Yes'
                        OR CallPopLite = 'Yes'
                        OR FormRefresh = 'Yes'
                        OR Commands = 'Yes'
                        OR SpeedTest = 'Yes'
                    );";

            try
            {
                DataTable dt = salesRepository.ExecuteSqlQuery(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

            if (oldScrollIndex >= 0 && oldScrollIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = oldScrollIndex;
            }
        }

        public void UpdateColumnToYes(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                return;

            var machineNames = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    object val = row.Cells["MachineName"].Value;
                    string machineName = val?.ToString();
                    if (!string.IsNullOrEmpty(machineName))
                        machineNames.Add(machineName);
                }
            }

            if (machineNames.Count == 0)
                return;

            try
            {
                var parameters = new Dictionary<string, object>();
                var paramNames = new List<string>();
                for (int i = 0; i < machineNames.Count; i++)
                {
                    string paramName = "@p" + i;
                    paramNames.Add(paramName);
                    parameters.Add(paramName, machineNames[i]);
                }
                string inClause = string.Join(", ", paramNames);
                string query = $@"
                    UPDATE Computers
                    SET {columnName} = 'Yes'
                    WHERE MachineName IN ({inClause});";

                int rowsUpdated = salesRepository.ExecuteSqlNonQuery(query, parameters);
                MessageBox.Show($"'{columnName}' set to 'Yes' for {rowsUpdated} rows.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating data: " + ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadSearchData(txtSearch.Text);
            }
            else
            {
                LoadData();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateColumnToYes("AppUpdate");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateColumnToYes("TillUpdater");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateColumnToYes("CallPopLite");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateColumnToYes("FormRefresh");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CommandAutoUpdateForm form = new CommandAutoUpdateForm();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateColumnToYes("SpeedTest");

            SpeedTestResultsForm resultsForm = new SpeedTestResultsForm();
            resultsForm.Show();
        }
    }
}
