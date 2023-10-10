using Identity.Domain.Constraints;
using Identity.Domain.Entities;
using Identity.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static System.Net.WebRequestMethods;
using Microsoft.IdentityModel.Logging;

namespace Identity.WebAPI.Extensions.Identity
{
    public static class IdentityExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
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
            services.AddIdentityServer(options =>
            {
                options.IssuerUri = config["IdentityServerSettings:IssuerUri"];
            })
                    .AddDeveloperSigningCredential()                    
                    .AddAspNetIdentity<AppUser>()
                    .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                    .AddInMemoryClients(IdentityServerConfig.Clients)
                    .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                    .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                    .AddProfileService<ProfileService>();

            services.AddLocalApiAuthentication();
        }

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:7124";
                    options.RequireHttpsMetadata = false;                    
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = "http://localhost",
                        ValidateIssuer = false,
                        ValidateAudience = false,                        
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
