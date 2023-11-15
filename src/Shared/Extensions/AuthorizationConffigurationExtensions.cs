using Microsoft.Extensions.DependencyInjection;
using Shared.IdentityConstraints;

namespace Shared.Extensions
{
    public static class AuthorizationConffigurationExtensions
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Policies.AdminOnly,
                    policy =>
                    {
                        policy.RequireRole(Roles.AdminRole.Name);
                    }
                );
            });
        }
    }
}
