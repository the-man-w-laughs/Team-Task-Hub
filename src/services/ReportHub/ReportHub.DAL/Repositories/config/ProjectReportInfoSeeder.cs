using ReportHub.DAL.Models;
using Shared.Repository.NoSql;

namespace ReportHub.DAL.Repositories
{
    public class ProjectReportInfoSeeder : IMongoDbSeeder<ProjectReportInfo>
    {
        public IEnumerable<ProjectReportInfo> Seed()
        {
            var projectReports = new List<ProjectReportInfo>
            {
                new ProjectReportInfo
                {
                    ProjectAuthorId = 1,
                    ProjectId = 1,
                    UpdatedAt = DateTime.Now.AddMinutes(-30),
                    CreatedAt = DateTime.Now.AddMinutes(-30),
                    Reports = new List<Report>()
                }
            };

            return projectReports;
        }
    }
}
