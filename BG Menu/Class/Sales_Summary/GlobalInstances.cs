using BG_Menu.Data;
using System;
using System.Data;
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
            if (IsOfflineMode)
            {
                // Still create the WeekDateManager so date logic works,
                // but don’t wire up the repository connection.
                WeekDateManager = await WeekDateManager.CreateAsync();
                SalesRepository = new SalesRepository(WeekDateManager);
                return;
            }

            WeekDateManager = await WeekDateManager.CreateAsync();
            SalesRepository = new SalesRepository(WeekDateManager);
        }

        public static async Task TryLoadSalesDataAsync()
        {
            if (IsOfflineMode)
            {
                // In offline mode, just give an empty table
                GlobalSalesData = new DataTable();
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
    }
}
