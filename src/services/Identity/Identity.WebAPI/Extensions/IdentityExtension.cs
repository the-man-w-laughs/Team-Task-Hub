using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Identity.WebAPI.Extensions
{
    public static class IdentityExtension
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentityServer()
             .AddInMemoryClients(new List<Client>())
             .AddInMemoryIdentityResources(new List<IdentityResource>())
             .AddInMemoryApiResources(new List<ApiResource>())
             .AddInMemoryApiScopes(new List<ApiScope>())
             .AddTestUsers(new List<TestUser>())
             .AddDeveloperSigningCredential();
        }
    }
}
