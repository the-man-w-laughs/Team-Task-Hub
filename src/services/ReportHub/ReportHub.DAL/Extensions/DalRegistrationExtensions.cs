using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportHub.DAL.Models;
using ReportHub.DAL.Repositories;
using Shared.Repository.NoSql;
using TeamHub.DAL.Repositories;

namespace ReportHub.DAL.Extensions;

public static class DalRegistrationExtensions
{
    public static IServiceCollection ConfigureMongoDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MongoDbSettings<ProjectReportInfo>>(configuration.GetSection("MongoDb"));
        services.AddSingleton<IMongoDbSeeder<ProjectReportInfo>, ProjectReportInfoSeeder>();
        services.AddScoped<IProjectReportInfoRepository, ProjectReportInfoRepository>();

        return services;
    }
}
