using Identity.Application.AutoMapperProfiles;
using Microsoft.Extensions.DependencyInjection;
using Identity.Application.Ports.Services;
using Identity.Application.Services;

namespace Identity.Application
{
    public static class ApplicationRegisterExtensions
    {
        public static void RegisterApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UsersProfile));

            services.AddScoped<IUserService, UserService>();
        }
    }
}
