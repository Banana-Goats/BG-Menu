using BG_Menu.Data;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BG_Menu.Class.Sales_Summary
{
    public static class GlobalInstances
    {
        public static bool IsOfflineMode { get; private set; } = false;

        public static void UseOfflineMode()
        {
            IsOfflineMode = true;
        }

        public static WeekDateManager WeekDateManager { get; private set; }
        public static SalesRepository SalesRepository { get; private set; }
        public static DataTable GlobalSalesData { get; set; }

        public static async Task InitializeAsync()
        {
            if (IsOfflineMode) return;
            WeekDateManager = await WeekDateManager.CreateAsync();
            SalesRepository = new SalesRepository(WeekDateManager);
        }

        public static async Task TryLoadSalesDataAsync()
        {
            if (IsOfflineMode)
            {
                // In offline mode, just give an empty table
                GlobalSalesData = new DataTable();
                MessageBox.Show("Offline mode is enabled. No data will be loaded.");
                return;
            }

            try
            {
                GlobalSalesData = await SalesRepository.GetHanaSalesDataAsync();
            }
            catch (Exception ex)
            {
                // Log the error and potentially update UI to alert the user.
                System.Diagnostics.Debug.WriteLine($"Error retrieving HANA sales data: {ex.Message}");

                // Set GlobalSalesData to an empty DataTable or cached data.
                GlobalSalesData = new DataTable();
            }
        }

        public static class HanaHealthCheck
        {

            public static async Task<bool> IsServerReachableAsync(string hostnameOrIp, int timeoutMs = 1000)
            {
                try
                {
                    using (var p = new Ping())
                    {
                        var reply = await p.SendPingAsync(hostnameOrIp, timeoutMs);
                        return reply.Status == IPStatus.Success;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
