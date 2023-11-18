using Microsoft.Extensions.DependencyInjection;
using Identity.Application.Ports.Utils;
using Identity.Application.Utils;

namespace Identity.Application
{
    public static class UtilsRegistrationExtensions
    {
        public static void RegisterUtilsDependencies(this IServiceCollection services)
        {
            services.AddScoped<IConfirmationEmailSender, ConfirmationEmailSender>();
        }
    }
}
