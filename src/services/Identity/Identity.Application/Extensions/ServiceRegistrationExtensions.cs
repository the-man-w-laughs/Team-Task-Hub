using Microsoft.Extensions.DependencyInjection;
using Identity.Application.Ports.Services;
using Identity.Application.Services;

namespace Identity.Application
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
