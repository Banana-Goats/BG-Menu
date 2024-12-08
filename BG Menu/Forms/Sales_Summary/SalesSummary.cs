using System.Data;
using System.Globalization;
using System.Data.SQLite;
using System.Diagnostics;
using Sap.Data.Hana;
using System.ComponentModel;
using BG_Menu.Class;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using BG_Menu.Class.Sales_Summary;

namespace BG_Menu.Forms.Sales_Summary
{
    public partial class SalesSummary : Form
    {
        private DataTable dataTable;
        private StoreInfo StoreInfo;
        private WeekDateManager weekDateManager;
        private List<StoreTarget> storeTargets;
        private List<StoreSales> sales2023;
        private List<StoreSales> sales2022;
        private List<StoreSales> sales2021;
        private List<StoreSales> sales2020;
        private bool isComboBoxTriggeredSelection = false;
        private string connectionString = "Server=10.100.230.6:30015;UserID=ELLIOTRENNER;Password=Drop-Local-Poet-Knife-5";
        private ProgressCalculator progressCalculator;

        private readonly StorageClient _storageClient;
        private AuthService authService;

        private DateTime selectedDate = DateTime.Now;

        FirestoreDb db;

        public SalesSummary()
        {
            string hanaClientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HANA_Client_Dlls");
            Environment.SetEnvironmentVariable("PATH", hanaClientPath + ";" + Environment.GetEnvironmentVariable("PATH"));

            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = System.IO.Path.Combine(appFolderPath, "FireBase Creds.json");

            // Initialize Firebase App
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonFilePath);
            db = FirestoreDb.Create("ableworld-ho-menu");

            _storageClient = StorageClient.Create();

            InitializeComponent();
            StoreInfo = new StoreInfo();
            weekDateManager = new WeekDateManager();
            progressCalculator = new ProgressCalculator(weekDateManager);
            InitializeComboBox();
            InitializeListBox();
            LoadStaticData();

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem showTodayDateMenuItem = new ToolStripMenuItem("Todays Data");
            showTodayDateMenuItem.Click += ShowTodayDateMenuItem_Click; // Event handler for showing today's date
            contextMenu.Items.Add(showTodayDateMenuItem);

            // Assign context menu to button
            button1.ContextMenuStrip = contextMenu;
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
        }

        private DataTable CalculateNetForStores(
            DataTable dataTable, DateTime startDate, DateTime endDate,
            List<StoreInfo> storeInfos,
            List<StoreTarget> storeTargets,
            List<StoreSales> sales2023,
            List<StoreSales> sales2022,
            List<StoreSales> sales2021,
            List<StoreSales> sales2020,
            decimal engineeringActual,
            bool applyEngineeringAdjustment)
        {
            DataTable summaryTable = new DataTable();
            summaryTable.Columns.Add("Name", typeof(string));
            summaryTable.Columns.Add("Actual", typeof(decimal));
            summaryTable.Columns.Add("Target", typeof(decimal));
            summaryTable.Columns.Add("£ to Target", typeof(decimal));
            summaryTable.Columns.Add("% to Target", typeof(string)); // Changed to string for formatting

            if (chkShow2023.Checked) summaryTable.Columns.Add("2023", typeof(decimal));
            if (chkShow2022.Checked) summaryTable.Columns.Add("2022", typeof(decimal));
            if (chkShow2021.Checked) summaryTable.Columns.Add("2021", typeof(decimal));
            if (chkShow2020.Checked) summaryTable.Columns.Add("2020", typeof(decimal));

            List<DataRow> selectedStoreRows = new List<DataRow>();

            foreach (var store in storeInfos)
            {
                string storeName = store.StoreName;
                string[] warehouseNames = store.WarehouseNames ?? new string[0];
                string excludeItemGroup = store.ExcludeItemGroup;
                string includeItemGroup = store.IncludeItemGroup;
                decimal totalNet = 0;

                Debug.WriteLine($"Processing store: {storeName}");

                var filteredRows = dataTable.AsEnumerable()
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

                // Round totalNet to the nearest whole number
                totalNet = Math.Round(totalNet);

                int startWeek = weekDateManager.GetWeekNumber(startDate);
                int endWeek = weekDateManager.GetWeekNumber(endDate);
                var filteredTargets = storeTargets
                    .Where(t => t.Store == storeName && t.Week >= startWeek && t.Week <= endWeek)
                    .ToList();

                decimal totalTarget = filteredTargets.Sum(t => t.Target);
                decimal poundsToTarget = totalNet - totalTarget;
                string percentToTarget = totalTarget != 0 ? ((totalNet / totalTarget) * 100).ToString("F2") : "0.00";

                DataRow summaryRow = summaryTable.NewRow();
                summaryRow["Name"] = storeName;
                summaryRow["Actual"] = totalNet;
                summaryRow["Target"] = totalTarget;

                // Adding initial values to selectedStoreRows to apply engineering adjustment later
                if (storeName != "Engineering" && storeName != "Stairlifts" && storeName != "Specialist")
                {
                    selectedStoreRows.Add(summaryRow);
                }

                if (chkShow2023.Checked)
                {
                    var filteredSales2023 = sales2023
                        .Where(s => s.Store == storeName && s.Week >= startWeek && s.Week <= endWeek)
                        .ToList();
                    decimal totalSales2023 = filteredSales2023.Sum(s => s.Sales);
                    summaryRow["2023"] = totalSales2023.ToString("F2");
                }

                if (chkShow2022.Checked)
                {
                    var filteredSales2022 = sales2022
                        .Where(s => s.Store == storeName && s.Week >= startWeek && s.Week <= endWeek)
                        .ToList();
                    decimal totalSales2022 = filteredSales2022.Sum(s => s.Sales);
                    summaryRow["2022"] = totalSales2022.ToString("F2");
                }

                if (chkShow2021.Checked)
                {
                    var filteredSales2021 = sales2021
                        .Where(s => s.Store == storeName && s.Week >= startWeek && s.Week <= endWeek)
                        .ToList();
                    decimal totalSales2021 = filteredSales2021.Sum(s => s.Sales);
                    summaryRow["2021"] = totalSales2021.ToString("F2");
                }

                if (chkShow2020.Checked)
                {
                    var filteredSales2020 = sales2020
                        .Where(s => s.Store == storeName && s.Week >= startWeek && s.Week <= endWeek)
                        .ToList();
                    decimal totalSales2020 = filteredSales2020.Sum(s => s.Sales);
                    summaryRow["2020"] = totalSales2020.ToString("F2");
                }

                summaryTable.Rows.Add(summaryRow);
            }

            // Adjust the actual for each selected store based on its percentage of the total actual
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

            // Recalculate £ to Target and % to Target after the adjustment
            foreach (DataRow row in summaryTable.Rows)
            {
                decimal actual = row.Field<decimal>("Actual");
                decimal target = row.Field<decimal>("Target");
                row["£ to Target"] = actual - target;
                row["% to Target"] = target != 0 ? ((actual / target) * 100).ToString("F2") : "0.00";
            }

            return summaryTable;
        }

        private (int, int) CountStoresOverTarget(DataTable ukTable, DataTable companyTable, List<string> storesToExclude)
        {
            int countOverTarget = 0;
            int totalCount = 0;

            // Combine all rows from all tables
            var allRows = ukTable.AsEnumerable().Concat(companyTable.AsEnumerable());

            foreach (var row in allRows)
            {
                string storeName = row["Name"].ToString();
                if (!storesToExclude.Contains(storeName))
                {
                    string poundsToTargetStr = row["£ to Target"].ToString();
                    if (!poundsToTargetStr.Contains("-"))
                    {
                        countOverTarget++;
                    }
                    totalCount++;
                }
            }

            return (countOverTarget, totalCount);
        }

        private (int, int) CountStoresOverProgress(DataTable ukTable, DataTable companyTable, List<string> storesToExclude, double progress)
        {
            int countOverProgress = 0;
            int totalCount = 0;

            // Combine all rows from all tables
            var allRows = ukTable.AsEnumerable().Concat(companyTable.AsEnumerable());

            foreach (var row in allRows)
            {
                string storeName = row["Name"].ToString();
                if (!storesToExclude.Contains(storeName))
                {
                    string percentToTargetStr = row["% to Target"].ToString();
                    if (double.TryParse(percentToTargetStr, out double percentToTarget) && percentToTarget > progress)
                    {
                        countOverProgress++;
                    }
                    totalCount++;
                }
            }

            return (countOverProgress, totalCount);
        }

        private decimal GetEngineeringActual(DataTable dataTable, DateTime startDate, DateTime endDate, StoreInfo store)
        {
            string[] warehouseNames = store.WarehouseNames ?? new string[0];
            string excludeItemGroup = store.ExcludeItemGroup;
            decimal totalNet = 0;

            var filteredRows = dataTable.AsEnumerable()
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

        private void PreviousYearChange(object sender, EventArgs e)
        {
            UpdateData();
        }

        public List<StoreTarget> LoadStoreTargetsFromSQLite(string filePath)
        {
            var storeTargets = new List<StoreTarget>();
            string connectionString = $"Data Source={filePath};Version=3;";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("SQLite connection opened successfully.");

                    string query = "SELECT Store, Week, Target FROM StoreTargets";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var storeTarget = new StoreTarget
                            {
                                Store = reader["Store"].ToString(),
                                Week = Convert.ToInt32(reader["Week"]),
                                Target = Convert.ToDecimal(reader["Target"])
                            };
                            storeTargets.Add(storeTarget);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"An error occurred while loading store targets from SQLite: {ex.Message}";
                Debug.WriteLine(errorMsg);
                throw new Exception(errorMsg, ex);
            }

            return storeTargets;
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isComboBoxTriggeredSelection = true; // Set the flag
            string selectedMonth = comboBox1.SelectedItem.ToString();
            var weeks = weekDateManager.GetWeeksForMonth(selectedMonth);

            listBoxWeeks.BeginUpdate();
            listBoxWeeks.ClearSelected();
            foreach (var week in weeks)
            {
                listBoxWeeks.SetSelected(week - 1, true);
            }
            listBoxWeeks.EndUpdate();

            // Ensure that all weeks are selected before proceeding
            await EnsureAllWeeksSelected(weeks);

            isComboBoxTriggeredSelection = false; // Reset the flag
        }

        private async void listBoxWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isComboBoxTriggeredSelection)
            {
                var selectedMonth = comboBox1.SelectedItem.ToString();
                var weeks = weekDateManager.GetWeeksForMonth(selectedMonth);

                // Ensure that all weeks are selected before calling UpdateData
                if (AreAllWeeksSelected(weeks))
                {
                    await UpdateData();
                }
            }
            else
            {
                // Manual selection
                await UpdateData();
            }
        }

        private async Task EnsureAllWeeksSelected(IEnumerable<int> weeks)
        {
            while (true)
            {
                if (AreAllWeeksSelected(weeks))
                {
                    break;
                }
                await Task.Delay(50); // Wait and check again
            }
        }

        private bool AreAllWeeksSelected(IEnumerable<int> weeks)
        {
            return weeks.All(week => listBoxWeeks.SelectedItems.Contains(week));
        }

        private async Task UpdateData()
        {
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

            // Calculate the engineering actual
            var engineeringStore = ukStoreMapping.FirstOrDefault(s => s.StoreName == "Engineering");
            decimal engineeringActual = 0;
            if (engineeringStore != null)
            {
                engineeringActual = GetEngineeringActual(dataTable, startDate, endDate, engineeringStore);
            }

            var ukStoreSummaryTable = await Task.Run(() => CalculateNetForStores(dataTable, startDate, endDate, ukStoreMapping, storeTargets, sales2023, sales2022, sales2021, sales2020, engineeringActual, true));
            var franchiseStoreSummaryTable = await Task.Run(() => CalculateNetForStores(dataTable, startDate, endDate, franchiseStoreMapping, storeTargets, sales2023, sales2022, sales2021, sales2020, engineeringActual, false));
            var companySummaryTable = await Task.Run(() => CalculateNetForStores(dataTable, startDate, endDate, companyMapping, storeTargets, sales2023, sales2022, sales2021, sales2020, engineeringActual, false));

            // Add aggregated stores to the company summary table
            AddAggregatedStores(companySummaryTable, aggregatedStoreMapping, ukStoreSummaryTable, franchiseStoreSummaryTable, companySummaryTable);

            // Filter data based on allowed stores
            var allowedStores = await GetAllowedStores();

            ukStoreSummaryTable = FilterDataTableByAllowedStores(ukStoreSummaryTable, allowedStores);
            franchiseStoreSummaryTable = FilterDataTableByAllowedStores(franchiseStoreSummaryTable, allowedStores);
            companySummaryTable = FilterDataTableByAllowedStores(companySummaryTable, allowedStores);

            double progress = progressCalculator.CalculateProgressThroughSelectedWeeks(selectedWeeks);

            // List of stores to exclude from counting
            List<string> storesToExclude = new List<string> { "Engineering", "DropShip", "UK", "Company Total", "Franchise Total", "North Region", "South Region", "North A", "North B", "South A", "South B", "Cheltenham" };

            // Count stores over target
            var (countOverTarget, totalCount) = CountStoresOverTarget(ukStoreSummaryTable, companySummaryTable, storesToExclude);

            var (countOverProgress, totalStores) = CountStoresOverProgress(ukStoreSummaryTable, companySummaryTable, storesToExclude, progress);

            int currentWeekNumber = GetCurrentWeekNumber();

            Invoke((Action)(() =>
            {
                dataGridViewUKStores.DataSource = ukStoreSummaryTable;
                SortDataGridViewByColumn(dataGridViewUKStores, "£ to Target");
                HighlightRowsBasedOnProgress(dataGridViewUKStores, progress);

                dataGridViewFranchiseStores.DataSource = franchiseStoreSummaryTable;
                SortDataGridViewByColumn(dataGridViewFranchiseStores, "£ to Target");
                HighlightRowsBasedOnProgress(dataGridViewFranchiseStores, progress);

                dataGridViewCompanyStores.DataSource = companySummaryTable;
                SortDataGridViewByColumn(dataGridViewCompanyStores, "£ to Target");
                HighlightRowsBasedOnProgress(dataGridViewCompanyStores, progress);

                DataGridViewSetup.DisableColumnSorting(dataGridViewUKStores, dataGridViewFranchiseStores, dataGridViewCompanyStores);
                DataGridViewSetup.ClearCurrentCell(dataGridViewUKStores, dataGridViewFranchiseStores, dataGridViewCompanyStores);

                // Update the label with the count
                lblStoresOverTarget.Text = $"{countOverTarget} / {totalCount} Profit Centres";

                lblStoresOverProgress.Text = $"{countOverProgress} / {totalStores} Profit Centres On Target";

                lblCurrentWeek.Text = $"Current Week: {currentWeekNumber}";
            }));
        }

        private void AddAggregatedStores(DataTable summaryTable, List<StoreInfo> aggregatedStoreInfos, DataTable ukStoreSummaryTable, DataTable franchiseStoreSummaryTable, DataTable companySummaryTable)
        {
            foreach (var aggregatedStore in aggregatedStoreInfos)
            {
                string aggregatedStoreName = aggregatedStore.StoreName;
                var storeNames = aggregatedStore.StoreNames;
                var includeTargetOnlyStoreNames = aggregatedStore.IncludeTargetOnlyStoreNames;

                var aggregatedActual = 0m;
                var aggregatedTarget = 0m;
                var aggregated2023 = 0m;
                var aggregated2022 = 0m;
                var aggregated2021 = 0m;
                var aggregated2020 = 0m;

                var sourceTables = new List<DataTable> { ukStoreSummaryTable, franchiseStoreSummaryTable, companySummaryTable };

                // Process StoreNames
                foreach (var storeName in storeNames)
                {
                    foreach (var table in sourceTables)
                    {
                        var storeRow = table.AsEnumerable().FirstOrDefault(row => row.Field<string>("Name") == storeName);
                        if (storeRow != null)
                        {
                            aggregatedActual += storeRow.Field<decimal>("Actual");
                            aggregatedTarget += storeRow.Field<decimal>("Target");
                            if (chkShow2023.Checked) aggregated2023 += storeRow.Field<decimal>("2023");
                            if (chkShow2022.Checked) aggregated2022 += storeRow.Field<decimal>("2022");
                            if (chkShow2021.Checked) aggregated2021 += storeRow.Field<decimal>("2021");
                            if (chkShow2020.Checked) aggregated2020 += storeRow.Field<decimal>("2020");
                        }
                    }
                }

                // Process IncludeTargetOnlyStoreNames
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
                                if (chkShow2023.Checked) aggregated2023 += targetOnlyStoreRow.Field<decimal>("2023");
                                if (chkShow2022.Checked) aggregated2022 += targetOnlyStoreRow.Field<decimal>("2022");
                                if (chkShow2021.Checked) aggregated2021 += targetOnlyStoreRow.Field<decimal>("2021");
                                if (chkShow2020.Checked) aggregated2020 += targetOnlyStoreRow.Field<decimal>("2020");
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
                if (chkShow2023.Checked) newRow["2023"] = aggregated2023;
                if (chkShow2022.Checked) newRow["2022"] = aggregated2022;
                if (chkShow2021.Checked) newRow["2021"] = aggregated2021;
                if (chkShow2020.Checked) newRow["2020"] = aggregated2020;

                summaryTable.Rows.Add(newRow);
            }
        }

        private async void LoadStaticData()
        {
            try
            {
                // Use the user's AppData directory
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appDirectory = Path.Combine(appDataPath, "BG-Menu");
                string filePath = Path.Combine(appDirectory, "targets.db");

                // Ensure the directory exists
                if (!Directory.Exists(appDirectory))
                {
                    Directory.CreateDirectory(appDirectory);
                }

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Load Store Targets and Sales Data
                    storeTargets = await Task.Run(() => LoadStoreTargetsFromSQLite(filePath));
                    sales2023 = await Task.Run(() => LoadStoreSalesFromSQLite(filePath, "Sales2023"));
                    sales2022 = await Task.Run(() => LoadStoreSalesFromSQLite(filePath, "Sales2022"));
                    sales2021 = await Task.Run(() => LoadStoreSalesFromSQLite(filePath, "Sales2021"));
                    sales2020 = await Task.Run(() => LoadStoreSalesFromSQLite(filePath, "Sales2020"));
                }
                else
                {
                    MessageBox.Show($"The file {filePath} does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the static data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<StoreSales> LoadStoreSalesFromSQLite(string filePath, string tableName)
        {
            var storeSales = new List<StoreSales>();
            string connectionString = $"Data Source={filePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Store, Week, Sales FROM {tableName}";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sales = new StoreSales
                        {
                            Store = reader["Store"].ToString(),
                            Week = Convert.ToInt32(reader["Week"]),
                            Sales = Convert.ToDecimal(reader["Sales"])
                        };
                        storeSales.Add(sales);
                    }
                }
            }

            return storeSales;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            listBoxWeeks.Enabled = true;
            
            // Refresh Data
            ExecuteAndDisplayQuery();
            SelectCurrentMonthInComboBox();
            UpdateData();
        }

        private int GetCurrentWeekNumber()
        {
            DateTime currentDate = DateTime.Now;
            return weekDateManager.GetWeekNumber(currentDate);
        }

        // Test function to test HANA connection
        private void TestConnection()
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection to SAP HANA database was successful.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to connect to SAP HANA database: " + ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExecuteAndDisplayQuery()
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                    SELECT 
                        T1.""TaxDate"",
                        T1.""ItmsGrpNam"",
                        T1.""WhsName"",
                        T1.""NET""
                    FROM 
                        ""sap.sboawuknewlive.cloud::MASTER_SALES"" T1
                    WHERE 
                        T1.""TaxDate"" >= '2024-09-01'
                    ORDER BY 
                        T1.""TaxDate"" ASC";

                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    dataTable = new DataTable();  // Assign to class-level variable
                    dataTable.Clear();
                    adapter.Fill(dataTable);

                    dataTable.Columns["TaxDate"].DataType = typeof(DateTime);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void SelectCurrentMonthInComboBox()
        {
            // Get the current week number from WeekDateManager
            int currentWeekNumber = weekDateManager.GetWeekNumber(DateTime.Now);

            // Loop through the dictionary to find the corresponding month for the current week number
            string currentMonth = null;
            foreach (var month in weekDateManager.GetMonthToWeeks())
            {
                if (month.Value.Contains(currentWeekNumber))
                {
                    currentMonth = month.Key;
                    break;
                }
            }

            // Select the found month in the comboBox
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
                        row.DefaultCellStyle.BackColor = Color.White; // Reset to default color
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

        private DataTable FilterDataTableByAllowedStores(DataTable dataTable, List<string> allowedStores)
        {
            if (allowedStores == null || allowedStores.Count == 0)
            {
                // If no allowed stores are found, return the original dataTable without any filtering
                return dataTable;
            }

            if (dataTable.Columns.Contains("Name"))
            {
                List<DataRow> rowsToDelete = new List<DataRow>();

                foreach (DataRow row in dataTable.Rows)
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

                dataTable.AcceptChanges();
            }
            else
            {
                MessageBox.Show("The DataTable does not contain the 'Name' column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
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
            if (newFontSize < 1) return; // Prevent font size from becoming too small

            Font newFont = new Font(dataGridView.Font.FontFamily, newFontSize);
            dataGridView.Font = newFont;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = newFont;
            dataGridView.RowsDefaultCellStyle.Font = newFont;
        }

        private void ShowTodayDateMenuItem_Click(object sender, EventArgs e)
        {
            selectedDate = DateTime.Now.Date;

            // Refresh data with today's date
            ExecuteAndDisplayQueryWithSelectedDate();
            SelectCurrentMonthInComboBox();
            UpdateData();
        }

        private void ExecuteAndDisplayQueryWithSelectedDate()
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update the query to use the selected date
                    string query = $@"
                                    SELECT 
                                        T1.""TaxDate"",
                                        T1.""ItmsGrpNam"",
                                        T1.""WhsName"",
                                        T1.""NET""
                                    FROM 
                                        ""sap.sboawuknewlive.cloud::MASTER_SALES"" T1
                                    WHERE 
                                        T1.""TaxDate"" >= '{selectedDate:yyyy-MM-dd}'  -- Use selected date
                                    ORDER BY 
                                        T1.""TaxDate"" ASC";

                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    dataTable = new DataTable();  // Assign to class-level variable
                    dataTable.Clear();
                    adapter.Fill(dataTable);

                    dataTable.Columns["TaxDate"].DataType = typeof(DateTime);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }       
        
    }
}