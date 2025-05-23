﻿using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Sap.Data.Hana;
using BG_Menu.Class.Sales_Summary;


namespace BG_Menu.Data
{
    public class SalesRepository
    {
        private readonly string hanaConnectionString;
        private readonly string sqlConnectionString;
        private WeekDateManager weekDateManager;

        public SalesRepository(WeekDateManager wdm)
        {
            hanaConnectionString = ConfigurationManager.ConnectionStrings["Hana"].ConnectionString;
            sqlConnectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
            weekDateManager = wdm;
        }

        public async Task<DataTable> GetHanaSalesDataAsync(DateTime? selectedDate = null)
        {
            if (GlobalInstances.IsHanaOffline)
                return new DataTable(); // Empty table if HANA is offline

            DataTable dt = new DataTable();
            using (var connection = new HanaConnection(hanaConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    DateTime effectiveDate = selectedDate ?? await GetDefaultStartDateAsyncFromSql();

                    string query = $@"
                SELECT 
                    T1.""TaxDate"",
                    T1.""ItmsGrpNam"",
                    T1.""WhsName"",
                    T1.""NET""
                FROM 
                    ""sap.sboawuknewlive.cloud::MASTER_SALES"" T1
                WHERE 
                    T1.""TaxDate"" >= '{effectiveDate:yyyy-MM-dd}'
                ORDER BY 
                    T1.""TaxDate"" ASC";

                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    adapter.Fill(dt);
                    dt.Columns["TaxDate"].DataType = typeof(DateTime);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"HANA Error: {ex.Message}");
                }
            }
            return dt;
        }

        private async Task<DateTime> GetDefaultStartDateAsyncFromSql()
        {
            // Get the connection string from your configuration (assuming it is set in App.config/Web.config).
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                SELECT [Value] 
                FROM [Config].[AppConfigs] 
                WHERE [Application] = 'BG Menu' AND [Config] = 'Start Date'";

                    object result = await command.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                    {
                        string dateString = result.ToString();
                        DateTime parsedDate;
                        // Attempt to parse the date string.
                        if (DateTime.TryParse(dateString, out parsedDate))
                        {
                            return parsedDate;
                        }
                    }
                }
            }
            // Fallback if retrieval or parsing fails.
            return new DateTime(2025, 9, 1);
        }

        public DataTable GetSalesDataFromSQL()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Store, 
                        Week, 
                        Target, 
                        Sales2020, 
                        Sales2021, 
                        Sales2022,
                        Sales2023,
                        Sales2024
                    FROM Sales.SalesData";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ExecuteHanaQuery(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                using (HanaConnection connection = new HanaConnection(hanaConnectionString))
                {
                    connection.Open();
                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    adapter.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                MessageBox.Show("Something Went Wrong, Blame Elliot");
                return null;
            }            
        }

        public int ExecuteSqlNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                }
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteSqlQuery(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                }
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int ExecuteSqlScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                }
                con.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public void UpdateSalesDataCache(DataTable hanaData)
        {
            int financialYear = GetFinancialYear(DateTime.Now);
            string columnName = "Sales" + financialYear.ToString(); // e.g. "Sales2024"

            // Ensure the column exists in the SQL table; if not, create it.
            EnsureSalesColumnExists(columnName);

            // Combine all store mappings (UK, Franchise, Company)
            var allStores = StoreWarehouseMapping.GetUKStoreMapping()
                .Concat(StoreWarehouseMapping.GetFranchiseStoreMapping())
                .Concat(StoreWarehouseMapping.GetCompanyMapping())
                .Concat(StoreWarehouseMapping.GetAggregatedStoreMapping());

            foreach (var storeInfo in allStores)
            {
                // Aggregate HANA data by week for the given store
                List<StoreSales> aggregatedSales = GetWeeklySalesFromHanaData(hanaData, storeInfo.StoreName, storeInfo);

                // Update the SQL table for each week where data was found
                foreach (var sale in aggregatedSales)
                {
                    UpdateSqlSales(storeInfo.StoreName, sale.Week, sale.Sales, columnName);
                }
            }
        }

        private int GetFinancialYear(DateTime date)
        {
            return date.Month >= 9 ? date.Year : date.Year - 1;
        }

        private void UpdateSqlSales(string store, int week, decimal sales, string columnName)
        {
            string query = $@"
                UPDATE Sales.SalesData 
                SET {columnName} = @sales 
                WHERE Store = @store AND Week = @week";

            var parameters = new Dictionary<string, object>
            {
                { "@sales", sales },
                { "@store", store },
                { "@week", week }
            };

            ExecuteSqlNonQuery(query, parameters);
        }

        private void EnsureSalesColumnExists(string columnName)
        {
            string query = $@"
                IF NOT EXISTS (
                    SELECT * 
                    FROM sys.columns 
                    WHERE Name = '{columnName}' 
                      AND Object_ID = OBJECT_ID('Sales.SalesData')
                )
                BEGIN
                    ALTER TABLE Sales.SalesData ADD {columnName} DECIMAL(18,2) NULL;
                END";
            ExecuteSqlNonQuery(query);
        }

        private List<StoreSales> GetWeeklySalesFromHanaData1(DataTable hanaData, string storeName, StoreInfo storeInfo)
        {
            Dictionary<int, decimal> weekToSales = new Dictionary<int, decimal>();
            var warehouseNames = storeInfo?.WarehouseNames ?? Array.Empty<string>();
            string excludeItemGroup = storeInfo?.ExcludeItemGroup;

            foreach (DataRow row in hanaData.Rows)
            {
                string whsName = row["WhsName"].ToString();

                if (!warehouseNames.Contains(whsName, StringComparer.OrdinalIgnoreCase))
                    continue;

                if (!string.IsNullOrEmpty(excludeItemGroup))
                {
                    string itemGroup = row["ItmsGrpNam"].ToString();
                    if (itemGroup.Equals(excludeItemGroup, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                if (row["TaxDate"] is DateTime taxDate)
                {
                    int weekNumber = weekDateManager.GetWeekNumber(taxDate);
                    if (weekNumber == -1)
                        continue;

                    decimal net = 0;
                    if (decimal.TryParse(row["NET"].ToString(), out decimal parsedNet))
                        net = parsedNet;

                    if (!weekToSales.ContainsKey(weekNumber))
                        weekToSales[weekNumber] = 0;
                    weekToSales[weekNumber] += net;
                }
            }

            return weekToSales
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp => new StoreSales
                    {
                        Store = storeName,
                        Week = kvp.Key,
                        Sales = kvp.Value
                    })
                    .ToList();
        }

        private List<StoreSales> GetWeeklySalesFromHanaData(DataTable hanaData, string storeName, StoreInfo storeInfo)
        {
            // 1) Collect all warehouse names for this store (leaf + aggregates)
            var warehouseSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // 1a) Direct warehouses on this StoreInfo
            if (storeInfo?.WarehouseNames != null)
                foreach (var wh in storeInfo.WarehouseNames)
                    warehouseSet.Add(wh);

            // We'll need the three leaf lists to look up children:
            var leafStores = StoreWarehouseMapping
                                .GetUKStoreMapping()
                                .Concat(StoreWarehouseMapping.GetFranchiseStoreMapping())
                                .Concat(StoreWarehouseMapping.GetCompanyMapping());

            // 1b) Expand any StoreNames (for aggregated entries)
            if (storeInfo?.StoreNames != null)
            {
                foreach (var childName in storeInfo.StoreNames)
                {
                    var child = leafStores
                        .FirstOrDefault(si =>
                            si.StoreName.Equals(childName, StringComparison.OrdinalIgnoreCase));
                    if (child?.WarehouseNames != null)
                        foreach (var wh in child.WarehouseNames)
                            warehouseSet.Add(wh);
                }
            }

            // 1c) Also expand IncludeTargetOnlyStoreNames (e.g. Engineering group)
            if (storeInfo?.IncludeTargetOnlyStoreNames != null)
            {
                foreach (var onlyName in storeInfo.IncludeTargetOnlyStoreNames)
                {
                    var child = leafStores
                        .FirstOrDefault(si =>
                            si.StoreName.Equals(onlyName, StringComparison.OrdinalIgnoreCase));
                    if (child?.WarehouseNames != null)
                        foreach (var wh in child.WarehouseNames)
                            warehouseSet.Add(wh);
                }
            }

            // 2) Now apply your existing loop + exclude logic
            var weekToSales = new Dictionary<int, decimal>();
            string excludeItemGroup = storeInfo?.ExcludeItemGroup;

            foreach (DataRow row in hanaData.Rows)
            {
                var whsName = row["WhsName"].ToString();
                if (!warehouseSet.Contains(whsName))
                    continue;

                if (!string.IsNullOrEmpty(excludeItemGroup))
                {
                    var grp = row["ItmsGrpNam"].ToString();
                    if (grp.Equals(excludeItemGroup, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                if (row["TaxDate"] is DateTime taxDate)
                {
                    int wk = weekDateManager.GetWeekNumber(taxDate);
                    if (wk < 0) continue;

                    if (decimal.TryParse(row["NET"].ToString(), out var net))
                    {
                        if (!weekToSales.ContainsKey(wk))
                            weekToSales[wk] = 0;
                        weekToSales[wk] += net;
                    }
                }
            }

            // 3) Project into your DTO and return
            return weekToSales
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new StoreSales
                {
                    Store = storeName,
                    Week = kvp.Key,
                    Sales = kvp.Value
                })
                .ToList();
        }
    }
}