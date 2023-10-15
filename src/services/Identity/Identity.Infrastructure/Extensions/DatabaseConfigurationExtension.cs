using Identity.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Namespace;

namespace Identity.Infrastructure.Extensions
{
    public static class DbConfigurationExtension
    {
        public static void ConfigureDatabaseConnection(
            this IServiceCollection services,
            ConfigurationManager config
        )
        {
            var connectionString = config.GetConnectionString("Default");
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseMySQL(connectionString);
            });
            services.AddScoped<IAppUserRepository, AppUserRepository>();
        }
    }
}
