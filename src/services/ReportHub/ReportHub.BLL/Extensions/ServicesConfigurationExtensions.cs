using Microsoft.Extensions.DependencyInjection;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.services;

namespace ReportHub.BLL.Extensions;

public static class ServicesConfigurationExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectReportInfoService, ProjectReportInfoService>();

        return services;
    }
}
