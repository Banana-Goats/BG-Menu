using System;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using BG_Menu.Class;
using BG_Menu.Class.Sales_Summary;
using BG_Menu.Data;
using LiveChartsCore.Geo;
using System.Drawing;

namespace BG_Menu.Forms.Sales_Summary
{
    public partial class SalesSummary : Form
    {
        // Remove the local instance for HANA data; we will use GlobalInstances.GlobalSalesData instead.
        // private DataTable dataTable;
        private StoreInfo StoreInfo;
        private WeekDateManager weekDateManager;
        private SalesRepository salesRepository;
        private List<StoreTarget> storeTargets;
        // Instead of hardcoded sales lists, we now use a dictionary keyed by year.
        private Dictionary<string, List<StoreSales>> salesByYear = new Dictionary<string, List<StoreSales>>();
        private Panel panelYearOptions;
        // Dictionary to keep track of dynamically created checkboxes.
        private Dictionary<string, CheckBox> salesYearCheckBoxes = new Dictionary<string, CheckBox>();
        private bool isComboBoxTriggeredSelection = false;
        private ProgressCalculator progressCalculator;
        private AuthService authService;
        private DateTime selectedDate = DateTime.Now;
        FirestoreDb db;

        public bool OpenedInNewWindow { get; set; } = false;
        private System.Windows.Forms.Timer autoTimer;

        public SalesSummary()
        {
            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = System.IO.Path.Combine(appFolderPath, "FireBase Creds.json");

            // Initialize Firebase App
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonFilePath);
            db = FirestoreDb.Create("ableworld-ho-menu");

            InitializeComponent();
            StoreInfo = new StoreInfo();

            InitializeComboBox();
            InitializeListBox();            

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem showTodayDateMenuItem = new ToolStripMenuItem("Todays Data");
            showTodayDateMenuItem.Click += ShowTodayDateMenuItem_Click; // Event handler for showing today's date
            contextMenu.Items.Add(showTodayDateMenuItem);

            // Assign context menu to button.
            button1.ContextMenuStrip = contextMenu;
        }

        private async void SalesSummary_Load(object sender, EventArgs e)
        {
            // Use the global instances which should already be initialized.
            weekDateManager = GlobalInstances.WeekDateManager;
            salesRepository = GlobalInstances.SalesRepository;
            progressCalculator = new ProgressCalculator(weekDateManager);

            LoadStaticData();

            if (GlobalInstances.IsOfflineMode)
            {
                btnToggleYearOptions.Enabled = false;
            }
        }

        private void InitializeComboBox()
        {
            string[] months = new string[]
            {
                "September", "October", "November", "December",
                "January", "February", "March", "April",
                "May", "June", "July", "August"
            };

            comboBox1.Items.AddRange(months);
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void InitializeListBox()
        {
            for (int i = 1; i <= 53; i++)
            {
                listBoxWeeks.Items.Add(i);
            }
            listBoxWeeks.SelectedIndexChanged += listBoxWeeks_SelectedIndexChanged;
            listBoxWeeks.Enabled = false;
        }

        private void InitializeYearOptionsPanelDynamic(List<string> salesYears)
        {
            panelYearOptions = new Panel
            {
                Size = new Size(200, salesYears.Count * 25 + 10),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                Location = new Point(btnToggleYearOptions.Left, btnToggleYearOptions.Bottom + 5)
            };

            salesYearCheckBoxes.Clear();
            int yPos = 10;
            foreach (var year in salesYears)
            {
                CheckBox chk = new CheckBox
                {
                    Text = year,
                    Location = new Point(10, yPos),
                    AutoSize = true
                };
                chk.CheckedChanged += chkYearOption_CheckedChanged;
                salesYearCheckBoxes[year] = chk;
                panelYearOptions.Controls.Add(chk);
                yPos += 25;
            }
            this.Controls.Add(panelYearOptions);
        }

        private void btnToggleYearOptions_Click(object sender, EventArgs e)
        {
            // Toggle the panel's visibility.
            panelYearOptions.Visible = !panelYearOptions.Visible;
            btnToggleYearOptions.Text = panelYearOptions.Visible ? "Hide Options" : "Show Options";

            if (panelYearOptions.Visible)
            {
                panelYearOptions.BringToFront();
            }
            panelYearOptions.Location = new Point(btnToggleYearOptions.Left, btnToggleYearOptions.Bottom + 5);
        }

        private void chkYearOption_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private DataTable CalculateNetForStores(
            DataTable data, DateTime startDate, DateTime endDate,
            List<StoreInfo> storeInfos,
            List<StoreTarget> storeTargets,
            Dictionary<string, List<StoreSales>> salesByYear,
            decimal engineeringActual,
            bool applyEngineeringAdjustment)
        {
            DataTable summaryTable = new DataTable();
            summaryTable.Columns.Add("Name", typeof(string));
            summaryTable.Columns.Add("Actual", typeof(decimal));
            summaryTable.Columns.Add("Target", typeof(decimal));
            summaryTable.Columns.Add("£ to Target", typeof(decimal));
            summaryTable.Columns.Add("% to Target", typeof(string)); // For formatted percentage

            // For each checked sales year add two columns: one for the sales value and one for the percentage difference.
            foreach (var kvp in salesYearCheckBoxes)
            {
                if (kvp.Value.Checked)
                {
                    summaryTable.Columns.Add(kvp.Key, typeof(decimal));
                    summaryTable.Columns.Add($"% Diff {kvp.Key}", typeof(string));
                }
            }

            List<DataRow> selectedStoreRows = new List<DataRow>();

            foreach (var store in storeInfos)
            {
                string storeName = store.StoreName;
                string[] warehouseNames = store.WarehouseNames ?? new string[0];
                string excludeItemGroup = store.ExcludeItemGroup;
                string includeItemGroup = store.IncludeItemGroup;
                decimal totalNet = 0;

                Debug.WriteLine($"Processing store: {storeName}");

                var filteredRows = data.AsEnumerable()
                    .Where(row => row.Field<DateTime>("TaxDate") >= startDate
                                  && row.Field<DateTime>("TaxDate") <= endDate
                                  && warehouseNames.Contains(row.Field<string>("WhsName"), StringComparer.OrdinalIgnoreCase)
                                  && (excludeItemGroup == null || row.Field<string>("ItmsGrpNam") != excludeItemGroup)
                                  && (includeItemGroup == null || row.Field<string>("ItmsGrpNam") == includeItemGroup));

                foreach (var row in filteredRows)
                {
                    if (decimal.TryParse(row.Field<object>("NET")?.ToString(), out decimal netValue))
                    {
                        totalNet += netValue;
                    }
                }

                totalNet = Math.Round(totalNet);

                int startWeek = weekDateManager.GetWeekNumber(startDate);
                int endWeek = weekDateManager.GetWeekNumber(endDate);
                var filteredTargets = storeTargets
                    .Where(t => t.Store == storeName && t.Week >= startWeek && t.Week <= endWeek)
                    .ToList();

                decimal totalTarget = filteredTargets.Sum(t => t.Target);
                string percentToTarget = totalTarget != 0 ? ((totalNet / totalTarget) * 100).ToString("F2") : "0.00";

                DataRow summaryRow = summaryTable.NewRow();
                summaryRow["Name"] = storeName;
                summaryRow["Actual"] = totalNet;
                summaryRow["Target"] = totalTarget;

                // Exclude some stores from the engineering adjustment.
                if (storeName != "Engineering" && storeName != "Stairlifts" && storeName != "Specialist")
                {
                    selectedStoreRows.Add(summaryRow);
                }

                // Loop over each dynamic sales year.
                foreach (var kvp in salesByYear)
                {
                    string year = kvp.Key;
                    bool isChecked = salesYearCheckBoxes.ContainsKey(year) && salesYearCheckBoxes[year].Checked;
                    AddYearlySales(summaryRow, year, isChecked, kvp.Value, storeName, startWeek, endWeek);
                }

                // Calculate percentage differences for each sales year.
                foreach (var kvp in salesYearCheckBoxes)
                {
                    string year = kvp.Key;
                    if (kvp.Value.Checked && summaryTable.Columns.Contains(year) && summaryTable.Columns.Contains($"% Diff {year}"))
                    {
                        decimal salesValue = 0;
                        if (decimal.TryParse(summaryRow[year].ToString(), out salesValue))
                        {
                            decimal actual = summaryRow.Field<decimal>("Actual");
                            string diffFormatted = "0.00";
                            if (salesValue != 0)
                            {
                                decimal diff = ((actual - salesValue) / salesValue) * 100;
                                diffFormatted = diff.ToString("F2");
                            }
                            else
                            {
                                diffFormatted = "N/A";
                            }
                            summaryRow[$"% Diff {year}"] = diffFormatted;
                        }
                    }
                }

                summaryTable.Rows.Add(summaryRow);
            }

            if (applyEngineeringAdjustment)
            {
                decimal totalActual = selectedStoreRows.Sum(row => row.Field<decimal>("Actual"));
                foreach (DataRow row in selectedStoreRows)
                {
                    decimal actual = row.Field<decimal>("Actual");
                    decimal percentageOfTotal = totalActual != 0 ? actual / totalActual : 0;
                    decimal adjustment = percentageOfTotal * engineeringActual;
                    row["Actual"] = Math.Round(actual + adjustment);
                }
            }

            // Recalculate "£ to Target" and "% to Target"
            foreach (DataRow row in summaryTable.Rows)
            {
                decimal actual = row.Field<decimal>("Actual");
                decimal target = row.Field<decimal>("Target");
                row["£ to Target"] = actual - target;
                row["% to Target"] = target != 0 ? ((actual / target) * 100).ToString("F2") : "0.00";
            }

            return summaryTable;
        }

        private void AddYearlySales(DataRow row, string columnName, bool isChecked, List<StoreSales> salesData, string storeName, int startWeek, int endWeek)
        {
            if (!isChecked)
                return;

            decimal totalSales = salesData
                .Where(s => s.Store.Equals(storeName, StringComparison.OrdinalIgnoreCase)
                            && s.Week >= startWeek && s.Week <= endWeek)
                .Sum(s => s.Sales);
            row[columnName] = totalSales.ToString("F2");
        }

        private decimal GetEngineeringActual(DataTable data, DateTime startDate, DateTime endDate, StoreInfo store)
        {
            string[] warehouseNames = store.WarehouseNames ?? new string[0];
            string excludeItemGroup = store.ExcludeItemGroup;
            decimal totalNet = 0;

            var filteredRows = data.AsEnumerable()
                .Where(row => row.Field<DateTime>("TaxDate") >= startDate
                              && row.Field<DateTime>("TaxDate") <= endDate
                              && warehouseNames.Contains(row.Field<string>("WhsName"), StringComparer.OrdinalIgnoreCase)
                              && (excludeItemGroup == null || row.Field<string>("ItmsGrpNam") != excludeItemGroup));

            foreach (var row in filteredRows)
            {
                if (decimal.TryParse(row.Field<object>("NET")?.ToString(), out decimal netValue))
                {
                    totalNet += netValue;
                }
            }

            return Math.Round(totalNet);
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isComboBoxTriggeredSelection = true;
            string selectedMonth = comboBox1.SelectedItem.ToString();
            var weeks = weekDateManager.GetWeeksForMonth(selectedMonth);

            listBoxWeeks.BeginUpdate();
            listBoxWeeks.ClearSelected();
            foreach (var week in weeks)
            {
                listBoxWeeks.SetSelected(week - 1, true);
            }
            listBoxWeeks.EndUpdate();

            await EnsureAllWeeksSelected(weeks);
            isComboBoxTriggeredSelection = false;
        }

        private async void listBoxWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isComboBoxTriggeredSelection)
            {
                string selectedMonth = comboBox1.SelectedItem.ToString();
                var weeks = weekDateManager.GetWeeksForMonth(selectedMonth);
                if (AreAllWeeksSelected(weeks))
                {
                    await UpdateData();
                }
            }
            else
            {
                await UpdateData();
            }
        }

        private async Task EnsureAllWeeksSelected(IEnumerable<int> weeks)
        {
            while (true)
            {
                if (AreAllWeeksSelected(weeks))
                    break;
                await Task.Delay(50);
            }
        }

        private bool AreAllWeeksSelected(IEnumerable<int> weeks)
        {
            return weeks.All(week => listBoxWeeks.SelectedItems.Contains(week));
        }

        private void ClearAllGrids()
        {
            dataGridViewUKStores.DataSource = null;
            dataGridViewFranchiseStores.DataSource = null;
            dataGridViewCompanyStores.DataSource = null;
        }

        private async Task UpdateData()
        {
            if (GlobalInstances.IsOfflineMode)
            {
                // Optionally clear grids or show a message label on the form
                Invoke((Action)(() =>
                {
                    ClearAllGrids();
                    MessageBox.Show("Offline mode is enabled. No data will be loaded.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }));
                return;
            }

            var selectedWeeks = listBoxWeeks.SelectedItems.Cast<int>().ToList();
            if (selectedWeeks.Count == 0)
            {
                Debug.WriteLine("No weeks selected, exiting UpdateData.");
                return;
            }

            DateTime startDate = weekDateManager.GetStartDateForWeek(selectedWeeks.Min());
            DateTime endDate = weekDateManager.GetEndDateForWeek(selectedWeeks.Max());
            Debug.WriteLine($"Selected date range: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}");

            var ukStoreMapping = StoreWarehouseMapping.GetUKStoreMapping();
            var franchiseStoreMapping = StoreWarehouseMapping.GetFranchiseStoreMapping();
            var companyMapping = StoreWarehouseMapping.GetCompanyMapping();
            var aggregatedStoreMapping = StoreWarehouseMapping.GetAggregatedStoreMapping();

            var engineeringStore = ukStoreMapping.FirstOrDefault(s => s.StoreName == "Engineering");
            decimal engineeringActual = 0;
            if (engineeringStore != null)
            {
                engineeringActual = GetEngineeringActual(GlobalInstances.GlobalSalesData, startDate, endDate, engineeringStore);
            }

            var ukStoreSummaryTableTask = Task.Run(() =>
                CalculateNetForStores(GlobalInstances.GlobalSalesData, startDate, endDate, ukStoreMapping, storeTargets, salesByYear, engineeringActual, true));
            var franchiseStoreSummaryTableTask = Task.Run(() =>
                CalculateNetForStores(GlobalInstances.GlobalSalesData, startDate, endDate, franchiseStoreMapping, storeTargets, salesByYear, engineeringActual, false));
            var companySummaryTableTask = Task.Run(() =>
                CalculateNetForStores(GlobalInstances.GlobalSalesData, startDate, endDate, companyMapping, storeTargets, salesByYear, engineeringActual, false));

            DataTable ukStoreSummaryTable = await ukStoreSummaryTableTask;
            DataTable franchiseStoreSummaryTable = await franchiseStoreSummaryTableTask;
            DataTable companySummaryTable = await companySummaryTableTask;

            AddAggregatedStores(companySummaryTable, aggregatedStoreMapping, ukStoreSummaryTable, franchiseStoreSummaryTable, companySummaryTable);

            var allowedStores = await GetAllowedStores();
            ukStoreSummaryTable = FilterDataTableByAllowedStores(ukStoreSummaryTable, allowedStores);
            franchiseStoreSummaryTable = FilterDataTableByAllowedStores(franchiseStoreSummaryTable, allowedStores);
            companySummaryTable = FilterDataTableByAllowedStores(companySummaryTable, allowedStores);

            double progress = progressCalculator.CalculateProgressThroughSelectedWeeks(selectedWeeks);
            int currentWeekNumber = GetCurrentWeekNumber();

            Invoke((Action)(() =>
            {
                UpdateGridView(dataGridViewUKStores, ukStoreSummaryTable, progress);
                UpdateGridView(dataGridViewFranchiseStores, franchiseStoreSummaryTable, progress);
                UpdateGridView(dataGridViewCompanyStores, companySummaryTable, progress);

                DataGridViewSetup.DisableColumnSorting(dataGridViewUKStores, dataGridViewFranchiseStores, dataGridViewCompanyStores);
                DataGridViewSetup.ClearCurrentCell(dataGridViewUKStores, dataGridViewFranchiseStores, dataGridViewCompanyStores);

                lblCurrentWeek.Text = $"Current Week: {currentWeekNumber}";
            }));
        }

        private void UpdateGridView(DataGridView gridView, DataTable table, double progress)
        {
            gridView.DataSource = table;
            SortDataGridViewByColumn(gridView, "£ to Target");
            HighlightRowsBasedOnProgress(gridView, progress);
            FormatDataGridViewColumns(gridView);
        }

        private void FormatDataGridViewColumns(DataGridView gridView)
        {
            foreach (DataGridViewColumn column in gridView.Columns)
            {
                if (column.Name != "Name")
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.Format = "N0";
                }
            }
        }

        private void AddAggregatedStores(DataTable summaryTable, List<StoreInfo> aggregatedStoreInfos, DataTable ukStoreSummaryTable, DataTable franchiseStoreSummaryTable, DataTable companySummaryTable)
        {
            foreach (var aggregatedStore in aggregatedStoreInfos)
            {
                string aggregatedStoreName = aggregatedStore.StoreName;
                var storeNames = aggregatedStore.StoreNames;
                var includeTargetOnlyStoreNames = aggregatedStore.IncludeTargetOnlyStoreNames;

                decimal aggregatedActual = 0m;
                decimal aggregatedTarget = 0m;
                Dictionary<string, decimal> aggregatedSales = new Dictionary<string, decimal>();

                foreach (var year in salesYearCheckBoxes.Keys)
                {
                    if (salesYearCheckBoxes[year].Checked)
                        aggregatedSales[year] = 0m;
                }

                var sourceTables = new List<DataTable> { ukStoreSummaryTable, franchiseStoreSummaryTable, companySummaryTable };

                foreach (var storeName in storeNames)
                {
                    foreach (var table in sourceTables)
                    {
                        var storeRow = table.AsEnumerable().FirstOrDefault(row => row.Field<string>("Name") == storeName);
                        if (storeRow != null)
                        {
                            aggregatedActual += storeRow.Field<decimal>("Actual");
                            aggregatedTarget += storeRow.Field<decimal>("Target");
                            foreach (var year in aggregatedSales.Keys.ToList())
                            {
                                aggregatedSales[year] += storeRow.Table.Columns.Contains(year) ? storeRow.Field<decimal>(year) : 0m;
                            }
                        }
                    }
                }

                if (includeTargetOnlyStoreNames != null)
                {
                    foreach (var targetOnlyStoreName in includeTargetOnlyStoreNames)
                    {
                        foreach (var table in sourceTables)
                        {
                            var targetOnlyStoreRow = table.AsEnumerable().FirstOrDefault(row => row.Field<string>("Name") == targetOnlyStoreName);
                            if (targetOnlyStoreRow != null)
                            {
                                aggregatedTarget += targetOnlyStoreRow.Field<decimal>("Target");
                                foreach (var year in aggregatedSales.Keys.ToList())
                                {
                                    aggregatedSales[year] += targetOnlyStoreRow.Table.Columns.Contains(year)
                                        ? targetOnlyStoreRow.Field<decimal>(year)
                                        : 0m;
                                }
                            }
                        }
                    }
                }

                DataRow newRow = summaryTable.NewRow();
                newRow["Name"] = aggregatedStoreName;
                newRow["Actual"] = aggregatedActual;
                newRow["Target"] = aggregatedTarget;
                newRow["£ to Target"] = aggregatedActual - aggregatedTarget;
                newRow["% to Target"] = aggregatedTarget != 0 ? ((aggregatedActual / aggregatedTarget) * 100).ToString("F2") : "0.00";
                foreach (var kvp in aggregatedSales)
                {
                    newRow[kvp.Key] = kvp.Value;
                    decimal diff = kvp.Value != 0 ? ((aggregatedActual - kvp.Value) / kvp.Value) * 100 : 0;
                    newRow[$"% Diff {kvp.Key}"] = diff.ToString("F2");
                }

                summaryTable.Rows.Add(newRow);
            }
        }

        private async void LoadStaticData()
        {
            if (GlobalInstances.IsOfflineMode)
                return;

            try
            {
                DataTable salesData = salesRepository.GetSalesDataFromSQL();

                if (salesData != null && salesData.Rows.Count > 0)
                {
                    var salesColumns = salesData.Columns.Cast<DataColumn>()
                        .Select(c => c.ColumnName)
                        .Where(name => name.StartsWith("Sales") && name.Length > "Sales".Length)
                        .ToList();

                    foreach (var col in salesColumns)
                    {
                        string year = col.Substring("Sales".Length);
                        if (!salesByYear.ContainsKey(year))
                        {
                            salesByYear[year] = new List<StoreSales>();
                        }
                    }

                    InitializeYearOptionsPanelDynamic(salesByYear.Keys.ToList());

                    storeTargets = new List<StoreTarget>();

                    foreach (DataRow row in salesData.Rows)
                    {
                        string store = row["Store"].ToString();
                        int week = Convert.ToInt32(row["Week"]);

                        storeTargets.Add(new StoreTarget
                        {
                            Store = store,
                            Week = week,
                            Target = Convert.ToDecimal(row["Target"])
                        });

                        foreach (var col in salesColumns)
                        {
                            string year = col.Substring("Sales".Length);
                            decimal salesValue = 0;
                            decimal.TryParse(row[col].ToString(), out salesValue);

                            salesByYear[year].Add(new StoreSales
                            {
                                Store = store,
                                Week = week,
                                Sales = salesValue
                            });
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No sales data found in SQL table.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading static data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (GlobalInstances.IsOfflineMode)
            {
                MessageBox.Show("Offline mode – cannot refresh HANA data.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            button1.Enabled = false;
            comboBox1.Enabled = true;
            listBoxWeeks.Enabled = true;

            // Update the global HANA sales data.
            GlobalInstances.GlobalSalesData = await salesRepository.GetHanaSalesDataAsync();

            SelectCurrentMonthInComboBox();
            UpdateData();

            Task.Run(() =>
            {
                try
                {
                    salesRepository.UpdateSalesDataCache(GlobalInstances.GlobalSalesData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating sales data cache: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            button1.Enabled = true;
        }

        private int GetCurrentWeekNumber()
        {
            DateTime currentDate = DateTime.Now;
            return weekDateManager.GetWeekNumber(currentDate);
        }

        private void SelectCurrentMonthInComboBox()
        {
            int currentWeekNumber = weekDateManager.GetWeekNumber(DateTime.Now);
            string currentMonth = null;
            foreach (var month in weekDateManager.GetMonthToWeeks())
            {
                if (month.Value.Contains(currentWeekNumber))
                {
                    currentMonth = month.Key;
                    break;
                }
            }

            if (currentMonth != null)
            {
                int monthIndex = comboBox1.Items.IndexOf(currentMonth);
                if (monthIndex >= 0)
                {
                    comboBox1.SelectedIndex = monthIndex;
                }
                else
                {
                    MessageBox.Show($"Month '{currentMonth}' not found in ComboBox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Current week number does not belong to any month.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<int> selectedWeeks = listBoxWeeks.SelectedItems.Cast<int>().ToList();
            if (selectedWeeks.Count > 0)
            {
                double progress = progressCalculator.CalculateProgressThroughSelectedWeeks(selectedWeeks);
                MessageBox.Show($"Progress through selected weeks: {progress:F2}%", "Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No weeks selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HighlightRowsBasedOnProgress(DataGridView dataGridView, double progress)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["% to Target"].Value != null && decimal.TryParse(row.Cells["% to Target"].Value.ToString(), out decimal percentToTarget))
                {
                    if (percentToTarget >= 100)
                    {
                        row.DefaultCellStyle.BackColor = Color.GreenYellow;
                    }
                    else if (percentToTarget > (decimal)progress)
                    {
                        row.DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void SortDataGridViewByColumn(DataGridView dataGridView, string columnName)
        {
            if (dataGridView.Columns[columnName] != null)
            {
                dataGridView.Sort(dataGridView.Columns[columnName], ListSortDirection.Descending);
            }
        }

        private async Task<List<string>> GetAllowedStores()
        {
            try
            {
                authService = new AuthService(db);
                return await authService.GetAllowedStoresAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching allowed stores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<string>();
            }
        }

        private DataTable FilterDataTableByAllowedStores(DataTable data, List<string> allowedStores)
        {
            if (allowedStores == null || allowedStores.Count == 0)
            {
                return data;
            }

            if (data.Columns.Contains("Name"))
            {
                List<DataRow> rowsToDelete = new List<DataRow>();

                foreach (DataRow row in data.Rows)
                {
                    if (!allowedStores.Contains(row["Name"].ToString()))
                    {
                        rowsToDelete.Add(row);
                    }
                }

                foreach (var row in rowsToDelete)
                {
                    row.Delete();
                }

                data.AcceptChanges();
            }
            else
            {
                MessageBox.Show("The DataTable does not contain the 'Name' column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return data;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdjustFontSize(-1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdjustFontSize(1);
        }

        private void AdjustFontSize(int adjustment)
        {
            AdjustGridViewFontSize(dataGridViewUKStores, adjustment);
            AdjustGridViewFontSize(dataGridViewFranchiseStores, adjustment);
            AdjustGridViewFontSize(dataGridViewCompanyStores, adjustment);
        }

        private void AdjustGridViewFontSize(DataGridView dataGridView, int adjustment)
        {
            float newFontSize = dataGridView.Font.Size + adjustment;
            if (newFontSize < 1)
                return;
            Font newFont = new Font(dataGridView.Font.FontFamily, newFontSize);
            dataGridView.Font = newFont;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = newFont;
            dataGridView.RowsDefaultCellStyle.Font = newFont;
        }

        private async void ShowTodayDateMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalInstances.IsOfflineMode)
            {
                MessageBox.Show("Offline mode – cannot load today’s HANA data.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            selectedDate = DateTime.Now.Date;
            GlobalInstances.GlobalSalesData = await salesRepository.GetHanaSalesDataAsync(selectedDate);
            SelectCurrentMonthInComboBox();
            UpdateData();
        }  

    }
}
