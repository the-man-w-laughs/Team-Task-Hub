using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.Services
{
    public class ScheduledEmailService : IScheduledEmailService
    {
        public ScheduledEmailService(IMailingService mailingService, IHolidayService holidayService)
        { }

        public Task Schedule()
        {
            throw new NotImplementedException();
        }
    }
}
