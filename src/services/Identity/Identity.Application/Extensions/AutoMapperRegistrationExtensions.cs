using System.Reflection;
using Identity.Application.AutoMapperProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application
{
    public static class AutoMapperRegistrationExtensions
    {
        public static void RegisterAutomapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
