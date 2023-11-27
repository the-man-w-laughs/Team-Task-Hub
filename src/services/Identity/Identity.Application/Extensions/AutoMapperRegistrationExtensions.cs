using System.Reflection;
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
