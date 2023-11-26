using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Shared.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static IServiceCollection ConfigureLogging(
            this IServiceCollection services,
            WebApplicationBuilder builder
        )
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration().Enrich
                .FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticsearchSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            return services;
        }

        private static ElasticsearchSinkOptions ConfigureElasticsearchSink(
            IConfigurationRoot configuration,
            string environment
        )
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
            {
                AutoRegisterTemplate = true,
                IndexFormat =
                    $"{Assembly.GetExecutingAssembly()
                                         .GetName()
                                         .Name!.ToLower()
                                         .Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            };
        }
    }
}
