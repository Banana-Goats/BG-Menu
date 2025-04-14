using Microsoft.Data.SqlClient;
using System.Configuration;

public class WeekDateManager
{
    private Dictionary<string, int[]> monthToWeeks;
    private Dictionary<int, DateTime> weekToStartDate;
    private Dictionary<string, int[]> quarterToWeeks;

    public WeekDateManager()
    {
    }

    public static async Task<WeekDateManager> CreateAsync()
    {
        var manager = new WeekDateManager();
        await manager.InitializeDictionariesAsync();
        return manager;
    }

    private async Task InitializeDictionariesAsync()
    {
        // Retrieve the connection string from the configuration.
        string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

        // Initialize the collections.
        weekToStartDate = new Dictionary<int, DateTime>();
        var monthToWeeksList = new Dictionary<string, int[]>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            // Asynchronously build the weekToStartDate dictionary from Config.Weekdates.
            string queryWeekdates = "SELECT WeekNumber, StartDate FROM Config.Weekdates ORDER BY WeekNumber";
            using (SqlCommand cmd = new SqlCommand(queryWeekdates, connection))
            {
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int weekNumber = reader.GetInt32(0);
                        DateTime startDate = reader.GetDateTime(1);
                        weekToStartDate[weekNumber] = startDate;
                    }
                }
            }

            // Build the monthToWeeks dictionary from Config.MonthWeeks.
            // In this design each row contains MonthName and a comma-separated list of week numbers.
            string queryMonthWeeks = "SELECT MonthName, WeekNumbers FROM Config.MonthWeeks ORDER BY MonthName";
            using (SqlCommand cmd = new SqlCommand(queryMonthWeeks, connection))
            {
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string monthName = reader.GetString(0);
                        string weekNumbersStr = reader.GetString(1);
                        int[] weeks = weekNumbersStr
                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x.Trim()))
                            .ToArray();
                        monthToWeeksList[monthName] = weeks;
                    }
                }
            }
        }

        // Set the final dictionary.
        monthToWeeks = monthToWeeksList;
    }

    public int GetWeekNumber(DateTime date)
    {
        if (weekToStartDate == null)
        {
            throw new InvalidOperationException("Mapping data has not been loaded. Ensure initialization is complete.");
        }

        foreach (var kvp in weekToStartDate)
        {
            if (date >= kvp.Value && date < kvp.Value.AddDays(7))
            {
                return kvp.Key;
            }
        }
        return -1; // or throw an exception if no matching week found.
    }

    public int[] GetWeeksForMonth(string month)
    {
        return monthToWeeks.ContainsKey(month) ? monthToWeeks[month] : new int[0];
    }

    public DateTime GetStartDateForWeek(int week)
    {
        return weekToStartDate.ContainsKey(week) ? weekToStartDate[week] : DateTime.MinValue;
    }

    public DateTime GetEndDateForWeek(int week)
    {
        return weekToStartDate.ContainsKey(week) ? weekToStartDate[week].AddDays(6) : DateTime.MinValue;
    }

    public Dictionary<string, int[]> GetMonthToWeeks()
    {
        return monthToWeeks;
    }
}