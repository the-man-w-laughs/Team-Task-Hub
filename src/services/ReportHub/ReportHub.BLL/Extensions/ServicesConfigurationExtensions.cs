using Microsoft.Extensions.DependencyInjection;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Services;

namespace ReportHub.BLL.Extensions;

public static class ServicesConfigurationExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectInfoService, ProjectInfoService>();
        services.AddScoped<IProjectReportInfoService, ProjectReportInfoService>();
        services.AddScoped<IProjectReportService, ProjectReportService>();

        return services;
    }
}
