using Microsoft.Extensions.DependencyInjection;
using TeamHub.BLL.AutoMapperProfiles;

namespace TeamHub.BLL
{
    public static class BLLRegisterExtensions
    {
        public static void RegisterBLLDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectsProfile));
        }
    }
}
