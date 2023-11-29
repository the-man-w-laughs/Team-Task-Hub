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

        public async Task<IList<string>> GetAllUsersReportsPaths(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            var usersProjects = await GetAllAsync(
                offset: 0,
                limit: int.MaxValue,
                project => project.ProjectAuthorId == userId,
                cancellationToken: cancellationToken
            );

            return usersProjects
                .SelectMany(project => project.Reports.Select(report => report.Path))
                .ToList();
        }
    }
}
