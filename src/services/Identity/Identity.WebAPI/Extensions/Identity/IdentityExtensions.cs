using Identity.Domain.Constraints;
using Identity.Domain.Entities;
using Identity.Infrastructure.DatabaseContext;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Identity.WebAPI.Extensions.Identity
{
    public static class IdentityExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole<int>>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole<int>>();
        }
        public static void ConfigureIdentityServer(this IServiceCollection services, ConfigurationManager config)
        {
            var keyFilePath = config["IdentityServerSettings:SigningKeyPath"];
            var keyPassword = config["IdentityServerSettings:SigningKeyPassword"];

            var key = new X509Certificate2(keyFilePath, keyPassword);            

            services.AddIdentityServer(options =>
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

        public static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager config)
        {
            var keyFilePath = config["IdentityServerSettings:SigningKeyPath"];
            var keyPassword = config["IdentityServerSettings:SigningKeyPassword"];

            var key = new X509Certificate2(keyFilePath, keyPassword);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new X509SecurityKey(key)
                    };
                });
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AdminOnly, policy =>
                {
                    policy.RequireRole(Roles.AdminRole.Name);
                });
            });
        }
    }
}
