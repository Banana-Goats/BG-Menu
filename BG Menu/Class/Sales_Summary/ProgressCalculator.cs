public class ProgressCalculator
{
    private readonly WeekDateManager _weekDateManager;

    public ProgressCalculator(WeekDateManager weekDateManager)
    {
        _weekDateManager = weekDateManager;
    }

    public double CalculateProgressThroughSelectedWeeks(List<int> selectedWeeks)
    {
        if (!selectedWeeks.Any()) return 0;

        DateTime startDate = _weekDateManager.GetStartDateForWeek(selectedWeeks.Min());
        DateTime endDate = _weekDateManager.GetEndDateForWeek(selectedWeeks.Max()).AddDays(1).AddTicks(-1); // End of the last day

        DateTime now = DateTime.Now;

        // Calculate total business hours between startDate and endDate
        double totalBusinessHours = CalculateBusinessHoursBetweenDates(startDate, endDate);

        // Calculate business hours from startDate to now
        double businessHoursSoFar = CalculateBusinessHoursBetweenDates(startDate, now);

        // Calculate percentage progress
        return (businessHoursSoFar / totalBusinessHours) * 100;
    }

    private double CalculateBusinessHoursBetweenDates(DateTime start, DateTime end)
    {
        double businessHours = 0;
        DateTime current = start;

        while (current < end)
        {
            if (current.Hour >= 9 && current.Hour < 17) // During business hours
            {
                if (current.Date == end.Date && current.Hour >= end.Hour)
                {
                    businessHours += (end - current).TotalHours;
                }
                else
                {
                    businessHours += (new DateTime(current.Year, current.Month, current.Day, 17, 0, 0) - current).TotalHours;
                }
            }

            // Move to the next day
            current = current.Date.AddDays(1).AddHours(9);
        }

        return businessHours;
    }
}