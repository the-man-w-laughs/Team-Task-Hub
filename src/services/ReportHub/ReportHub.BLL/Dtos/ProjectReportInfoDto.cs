using ReportHub.DAL.Models;

namespace ReportHub.BLL.Dtos;

public class ProjectReportInfoDto
{
    public int ProjectId { get; set; }
    public int ProjectAuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Report> Reports { get; set; }
}
