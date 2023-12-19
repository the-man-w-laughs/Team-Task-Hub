using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class CorsExtensions
    {
        public static void ConfigureCors(
            this IServiceCollection services,
            ConfigurationManager config
        )
        {
            var angularClientOrigin = config["AllowedOrigins:AngularClientOrigin"];
            var apiGateway = config["AllowedOrigins:ApiGateway"];
            var reportHub = config["AllowedOrigins:ReportHub"];
            var teamHub = config["AllowedOrigins:TeamHub"];
            var identity = config["AllowedOrigins:Identity"];
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(angularClientOrigin, apiGateway, reportHub, teamHub, identity)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}
