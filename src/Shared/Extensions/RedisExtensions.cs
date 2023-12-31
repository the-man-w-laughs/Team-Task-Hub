using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Repository.NoSql.Redis;

namespace Shared.Extensions
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddUserRequestRepository(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration["Redis:ConnectionString"];
            services.AddSingleton<IUserRequestRepository>(
                provider => new UserRequestRepository(connectionString)
            );
            return services;
        }
    }
}
