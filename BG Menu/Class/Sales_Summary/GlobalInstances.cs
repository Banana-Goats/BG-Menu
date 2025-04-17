using BG_Menu.Data;
using System.Data;

public static class GlobalInstances
{
    public static WeekDateManager WeekDateManager { get; private set; }
    public static SalesRepository SalesRepository { get; private set; }
    public static DataTable GlobalSalesData { get; set; }

    public static async Task InitializeAsync()
    {
        WeekDateManager = await WeekDateManager.CreateAsync();
        SalesRepository = new SalesRepository(WeekDateManager);
        //GlobalSalesData = await SalesRepository.GetHanaSalesDataAsync();
    }

    public static async Task TryLoadSalesDataAsync()
    {
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