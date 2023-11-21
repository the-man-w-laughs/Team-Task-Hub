using Shared.Repository.NoSql;

namespace ReportHub.DAL.Models;

public class ProjectReportInfo : MongoBaseEntity
{
    public ProjectReportInfo(int ProjectId, int ProjectAuthorId)
    {
        this.ProjectId = ProjectId;
        this.ProjectAuthorId = ProjectAuthorId;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Reports = new();
    }

    public int ProjectId { get; set; }
    public int ProjectAuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Report> Reports { get; set; }
}
