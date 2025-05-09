using BG_Menu.Data;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BG_Menu.Class.Sales_Summary
{
    public static class GlobalInstances
    {
        public static bool IsHanaOffline { get; private set; } = false;

        public static void SetHanaOfflineMode()
        {
            IsHanaOffline = true;
        }

        public static WeekDateManager WeekDateManager { get; private set; }
        public static SalesRepository SalesRepository { get; private set; }
        public static DataTable GlobalSalesData { get; set; }


        public static async Task InitializeAsync()
        {            
            WeekDateManager = await WeekDateManager.CreateAsync();
            SalesRepository = new SalesRepository(WeekDateManager);
        }

        public static async Task TryLoadSalesDataAsync()
        {
            if (IsHanaOffline)
            {
                GlobalSalesData = new DataTable();
                MessageBox.Show("HANA offline mode is enabled. No HANA data loaded.");
                return;
            }

            try
            {
                GlobalSalesData = await SalesRepository.GetHanaSalesDataAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving HANA sales data: {ex.Message}");
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
