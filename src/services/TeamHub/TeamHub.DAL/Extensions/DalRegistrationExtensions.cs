using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamHub.DAL.DBContext;

namespace TeamHub.DAL.Extensions
{
    public static class DalRegistrationExtensions
    {
        public static void RegisterDalDependencies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<TeamHubDbContext>(builder =>
            {
                var connectionString = configuration.GetConnectionString("Default");
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Connection string cannot be empty.");
                builder.UseLazyLoadingProxies().UseMySQL(connectionString);
            });
        }
    }
}
