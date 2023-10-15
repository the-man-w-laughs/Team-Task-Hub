using Identity.Application.Ports.Repositories;
using Identity.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extensions
{
    public static class DatabaseConfigurationExtension
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

            services.AddScoped<IAuthDBContext>(x => x.GetService<AuthDbContext>());
        }
    }
}
