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
    public static class ApplicationRegisterExtensions
    {
        public static void RegisterApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UsersProfile));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<IEmailConfirmationHelper, EmailConfirmationHelper>();

            services.AddScoped<IUserService, UserService>();
        }
    }
}
