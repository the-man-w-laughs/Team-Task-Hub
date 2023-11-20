using Microsoft.Extensions.DependencyInjection;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Services;

namespace TeamHub.BLL
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IScheduledEmailService, ScheduledEmailService>();
        }
    }
}
