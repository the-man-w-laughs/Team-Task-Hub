using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Shared.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                options.AddSecurityDefinition(
                    "oauth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow()
                            {
                                TokenUrl = new Uri(config["IdentityServerSettings:Token"]),
                            }
                        }
                    }
                );

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = IdentityServerAuthenticationDefaults.AuthenticationScheme
                                },
                                Scheme = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header
                            },
                            new string[] { }
                        }
                    }
                );
            });
        }

        public static void UseSwaggerWithOAuth(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                string clientId = config["ClientSettings:ClientId"];
                string clientSecret = config["ClientSettings:ClientSecret"];
                options.OAuthClientId(clientId);
                options.OAuthClientSecret(clientSecret);
            });
        }
    }
}
