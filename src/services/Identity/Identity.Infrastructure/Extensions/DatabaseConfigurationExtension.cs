using Identity.Domain.Entities;
using Identity.Infrastructure.DbContext;
using Identity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
