using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.Services
{
    public class ScheduledEmailService : IScheduledEmailService
    {
        private readonly IMailingService _mailingService;
        private readonly IHolidayService _holidayService;

        public ScheduledEmailService(IMailingService mailingService, IHolidayService holidayService)
        {
            _mailingService = mailingService;
            _holidayService = holidayService;
        }

        public async Task Schedule()
        {
            if (!_holidayService.IsDayOff(DateTime.Now))
            {
                await _mailingService.SendPendingTasks();
            }
        }
    }
}
