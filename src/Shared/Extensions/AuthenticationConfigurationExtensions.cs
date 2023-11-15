using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Extensions;

public static class AuthenticationConfigurationExtensions
{
    public static void ConfigureAuthentication(
        this IServiceCollection services,
        ConfigurationManager config
    )
    {
        var keyFilePath = config["IdentityServerSettings:SigningKeyPath"];
        var keyPassword = config["IdentityServerSettings:SigningKeyPassword"];

        var key = new X509Certificate2(keyFilePath, keyPassword);

        services
            .AddAuthentication(auth =>
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
                options.RequireHttpsMetadata = false;
            });
    }
}
