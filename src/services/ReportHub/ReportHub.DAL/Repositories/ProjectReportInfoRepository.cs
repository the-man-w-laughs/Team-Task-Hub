using Microsoft.Extensions.Options;
using ReportHub.DAL.Models;
using Shared.Repository.NoSql;

namespace TeamHub.DAL.Repositories
{
    public class ProjectReportInfoRepository
        : MongoRepository<ProjectReportInfo>,
            IProjectReportInfoRepository
    {
        public ProjectReportInfoRepository(
            IOptions<MongoDbSettings<ProjectReportInfo>> mongoSettings,
            IMongoDbSeeder<ProjectReportInfo> mongoDbSeeder
        )
            : base(mongoSettings, mongoDbSeeder) { }
    }
}
