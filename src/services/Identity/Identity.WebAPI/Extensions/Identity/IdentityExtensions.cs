using Identity.Domain.Entities;
using Identity.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Identity.WebAPI.Extensions.Identity
{
    public static class IdentityExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services
                .AddIdentity<AppUser, IdentityRole<int>>(config =>
                {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole<int>>();
        }

        public static void ConfigureIdentityServer(
            this IServiceCollection services,
            ConfigurationManager config
        )
        {
            var keyFilePath = config["IdentityServerSettings:SigningKeyPath"];
            var keyPassword = config["IdentityServerSettings:SigningKeyPassword"];

            var key = new X509Certificate2(keyFilePath, keyPassword);

            services
                .AddIdentityServer(options =>
                {
                    options.IssuerUri = config["IdentityServerSettings:IssuerUri"];
                })
                .AddSigningCredential(key)
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(IdentityServerConfig.Clients)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddProfileService<ProfileService>();

            services.AddLocalApiAuthentication();
        }
    }
}
