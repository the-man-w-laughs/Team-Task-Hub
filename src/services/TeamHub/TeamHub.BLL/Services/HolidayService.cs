using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly List<DateTime> holidays;

        public HolidayService()
        {
            // Initialize the list of holidays
            holidays = new List<DateTime>
            {
                new DateTime(DateTime.Now.Year, 1, 1), // New Year's Day
                new DateTime(DateTime.Now.Year, 1, 2), // New Year (non-working day in Belarus)
                new DateTime(DateTime.Now.Year, 4, 1), // Easter Monday
                new DateTime(DateTime.Now.Year, 5, 1), // Labour Day
                new DateTime(DateTime.Now.Year, 5, 8), // Victory Day
                new DateTime(DateTime.Now.Year, 5, 9), // Victory Day (Soviet)
                new DateTime(DateTime.Now.Year, 7, 3), // Independence Day (Belarus)
                new DateTime(DateTime.Now.Year, 8, 15), // Assumption Day
                new DateTime(DateTime.Now.Year, 11, 1), // All Saints' Day
                new DateTime(DateTime.Now.Year, 11, 11), // Armistice Day
                new DateTime(DateTime.Now.Year, 12, 25) // Christmas Day
            };
        }

        public bool IsDayOff(DateTime date)
        {
            return IsHoliday(date) || IsWeekend(date);
        }

        private bool IsHoliday(DateTime date)
        {
            return holidays.Contains(date.Date);
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
