using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using ReportHub.DAL.Contracts;
using ReportHub.DAL.Repositories;
using ReportHub.DAL.Repositories.Config;

namespace ReportHub.WebApi.Extensions;

public static class MinioRegistratiionExtensions
{
    public static void ConfigureMinio(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MinioSettings>(configuration.GetSection("MinioConfig"));

        services.AddScoped<IMinioClient>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<MinioSettings>>().Value;
            var minio = new MinioClient()
                .WithEndpoint(settings.Endpoint)
                .WithCredentials(settings.AccessKey, settings.SecretKey)
                .Build();

            return minio;
        });

        services.AddScoped<IMinioRepository, MinioRepository>();
    }
}
