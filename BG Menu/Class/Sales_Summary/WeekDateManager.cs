public class WeekDateManager
{
    private Dictionary<string, int[]> monthToWeeks;
    private Dictionary<int, DateTime> weekToStartDate;

    public WeekDateManager()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        monthToWeeks = new Dictionary<string, int[]>
        {
            { "September", new int[] { 1, 2, 3, 4, 5 } },
            { "October", new int[] { 6, 7, 8, 9 } },
            { "November", new int[] { 10, 11, 12, 13 } },
            { "December", new int[] { 14, 15, 16, 17, 18 } },
            { "January", new int[] { 19, 20, 21, 22 } },
            { "February", new int[] { 23, 24, 25, 26 } },
            { "March", new int[] { 27, 28, 29, 30, 31 } },
            { "April", new int[] { 32, 33, 34, 35 } },
            { "May", new int[] { 36, 37, 38, 39 } },
            { "June", new int[] { 40, 41, 42 ,43, 44 } },
            { "July", new int[] { 45, 46, 47, 48 } },
            { "August", new int[] { 49, 50, 51, 52, 53 } }
        };

        weekToStartDate = new Dictionary<int, DateTime>
        {
            { 1, new DateTime(2024, 9, 1) },
            { 2, new DateTime(2024, 9, 8) },
            { 3, new DateTime(2024, 9, 15) },
            { 4, new DateTime(2024, 9, 22) },
            { 5, new DateTime(2024, 9, 29) },
            { 6, new DateTime(2024, 10, 6) },
            { 7, new DateTime(2024, 10, 13) },
            { 8, new DateTime(2024, 10, 20) },
            { 9, new DateTime(2024, 10, 27) },
            { 10, new DateTime(2024, 11, 3) },
            { 11, new DateTime(2024, 11, 10) },
            { 12, new DateTime(2024, 11, 17) },
            { 13, new DateTime(2024, 11, 24) },
            { 14, new DateTime(2024, 12, 1) },
            { 15, new DateTime(2024, 12, 8) },
            { 16, new DateTime(2024, 12, 15) },
            { 17, new DateTime(2024, 12, 22) },
            { 18, new DateTime(2024, 12, 29) },
            { 19, new DateTime(2025, 1, 5) },
            { 20, new DateTime(2025, 1, 12) },
            { 21, new DateTime(2025, 1, 19) },
            { 22, new DateTime(2025, 1, 26) },
            { 23, new DateTime(2025, 2, 2) },
            { 24, new DateTime(2025, 2, 9) },
            { 25, new DateTime(2025, 2, 16) },
            { 26, new DateTime(2025, 2, 23) },
            { 27, new DateTime(2025, 3, 2) },
            { 28, new DateTime(2025, 3, 9) },
            { 29, new DateTime(2025, 3, 16) },
            { 30, new DateTime(2025, 3, 23) },
            { 31, new DateTime(2025, 3, 30) },
            { 32, new DateTime(2025, 4, 6) },
            { 33, new DateTime(2025, 4, 13) },
            { 34, new DateTime(2025, 4, 20) },
            { 35, new DateTime(2025, 4, 27) },
            { 36, new DateTime(2025, 5, 4) },
            { 37, new DateTime(2025, 5, 11) },
            { 38, new DateTime(2025, 5, 18) },
            { 39, new DateTime(2025, 5, 25) },
            { 40, new DateTime(2025, 6, 1) },
            { 41, new DateTime(2025, 6, 8) },
            { 42, new DateTime(2025, 6, 15) },
            { 43, new DateTime(2025, 6, 22) },
            { 44, new DateTime(2025, 6, 29) },
            { 45, new DateTime(2025, 7, 6) },
            { 46, new DateTime(2025, 7, 13) },
            { 47, new DateTime(2025, 7, 20) },
            { 48, new DateTime(2025, 7, 27) },
            { 49, new DateTime(2025, 8, 3) },
            { 50, new DateTime(2025, 8, 10) },
            { 51, new DateTime(2025, 8, 17) },
            { 52, new DateTime(2025, 8, 24) },
            { 53, new DateTime(2025, 8, 31) }};
    }

    public (DateTime startDate, DateTime endDate) GetDateRangeForMonth(string month)
    {
        if (monthToWeeks.TryGetValue(month, out int[] weeks))
        {
            DateTime startDate = weekToStartDate[weeks.First()];
            DateTime endDate = weekToStartDate[weeks.Last()].AddDays(6);
            return (startDate, endDate);
        }
        return (DateTime.MinValue, DateTime.MinValue);
    }

    public int GetWeekNumber(DateTime date)
    {
        foreach (var kvp in weekToStartDate)
        {
            if (date >= kvp.Value && date < kvp.Value.AddDays(7))
            {
                return kvp.Key;
            }
        }
        return -1; // Invalid week
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