using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly List<DateTime> holidays;
        private readonly IHolidayRepository _holidayRepository;

        public HolidayService(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<bool> IsDayOffAsync(DateTime date)
        {
            return await IsHolidayAsync(date) || IsWeekend(date);
        }

        private async Task<bool> IsHolidayAsync(DateTime date)
        {
            var holidays = await _holidayRepository.GetAllAsync();

            return holidays.Any(holiday => holiday.Date.DayOfYear == date.DayOfYear);
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
