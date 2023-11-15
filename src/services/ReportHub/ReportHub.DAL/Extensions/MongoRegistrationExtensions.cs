using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportHub.DAL.Models;
using ReportHub.DAL.Repositories;
using Shared.Repository.NoSql;
using TeamHub.DAL.Repositories;

namespace ReportHub.DAL.Extensions;

public static class MongoRegistrationExtensions
{
    public static IServiceCollection ConfigureMongoDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var section = configuration.GetSection("MongoDb");
        services.Configure<MongoDbSettings<ProjectReportInfo>>(section);
        services.AddSingleton<IMongoDbSeeder<ProjectReportInfo>, ProjectReportInfoSeeder>();
        services.AddScoped<IProjectReportInfoRepository, ProjectReportInfoRepository>();

        return services;
    }
}
