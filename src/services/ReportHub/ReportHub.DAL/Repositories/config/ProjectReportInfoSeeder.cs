using ReportHub.DAL.Models;
using Shared.Repository.NoSql;

namespace ReportHub.DAL.Repositories
{
    public class ProjectReportInfoSeeder : IMongoDbSeeder<ProjectReportInfo>
    {
        public IEnumerable<ProjectReportInfo> Seed()
        {
            var projectReports = new List<ProjectReportInfo>();
            return projectReports;
        }
    }
}
