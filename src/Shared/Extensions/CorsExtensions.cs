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
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAngularClient",
                    builder =>
                    {
                        builder.WithOrigins(angularClientOrigin).AllowAnyHeader().AllowAnyMethod();
                    }
                );

                options.AddPolicy(
                    "AllowApiGateway",
                    builder =>
                    {
                        builder.WithOrigins(apiGateway).AllowAnyHeader().AllowAnyMethod();
                    }
                );
            });
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
        {
            return app.UseCors("AllowAngularClient").UseCors("AllowApiGateway");
        }
    }
}
