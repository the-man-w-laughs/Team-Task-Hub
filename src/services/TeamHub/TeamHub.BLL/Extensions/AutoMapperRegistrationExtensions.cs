using Microsoft.Extensions.DependencyInjection;
using TeamHub.BLL.AutoMapperProfiles;

namespace TeamHub.BLL
{
    public static class AutoMapperRegistrationExtensions
    {
        public static void RegisterAutomapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectsProfile));
            services.AddAutoMapper(typeof(UsersProfile));
        }
    }
}
