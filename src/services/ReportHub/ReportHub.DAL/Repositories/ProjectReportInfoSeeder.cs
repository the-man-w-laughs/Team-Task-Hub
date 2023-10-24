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
                    Reports = new List<Report>
                    {
                        new Report
                        {
                            Path = "report_2023_001",
                            GeneratedAt = DateTime.Now.AddMinutes(-30)
                        },
                        new Report
                        {
                            Path = "report_2023_002",
                            GeneratedAt = DateTime.Now.AddMinutes(-25)
                        }
                    }
                },
                new ProjectReportInfo
                {
                    ProjectAuthorId = 1,
                    ProjectId = 2,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now.AddMinutes(-25),
                    Reports = new List<Report>
                    {
                        new Report
                        {
                            Path = "report_2023_004",
                            GeneratedAt = DateTime.Now.AddMinutes(-15)
                        },
                        new Report
                        {
                            Path = "report_2023_005",
                            GeneratedAt = DateTime.Now.AddMinutes(-10)
                        }
                    }
                }

            };
            
            return projectReports;
        }
    }
}
