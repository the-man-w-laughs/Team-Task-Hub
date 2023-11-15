using Microsoft.Extensions.DependencyInjection;
using ReportHub.BLL.AutoMapperProfiles;

namespace ReportHub.BLL.Extensions
{
    public static class AutoMapperRegistrationExtensions
    {
        public static void RegisterAutomapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectsProfile));
        }
    }
}
