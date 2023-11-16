using Identity.Application.AutoMapperProfiles;
using Microsoft.Extensions.DependencyInjection;
using Identity.Application.Ports.Services;
using Identity.Application.Services;
using Identity.Application.Ports.Utils;
using Identity.Application.Utils;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Identity.Application
{
    public static class UtilsRegistrationExtensions
    {
        public static void RegisterUtilsDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEmailConfirmationHelper, EmailConfirmationHelper>();
        }
    }
}
