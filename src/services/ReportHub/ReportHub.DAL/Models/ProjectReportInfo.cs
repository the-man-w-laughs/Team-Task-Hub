using Shared.Repository.NoSql;

namespace ReportHub.DAL.Models;

public class ProjectReportInfo : MongoBaseEntity
{
    public int ProjectId { get; set; }
    public int ProjectAuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Report> Reports { get; set; }

    public ProjectReportInfo(int projectId, int projectAuthorId)
    {
        ProjectId = projectId;
        ProjectAuthorId = projectAuthorId;
        CreatedAt = DateTime.Now;
        Reports = new();
    }
}
