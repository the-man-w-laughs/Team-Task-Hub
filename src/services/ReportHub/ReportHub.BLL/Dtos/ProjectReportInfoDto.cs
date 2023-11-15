namespace ReportHub.BLL.Dtos;
public class ProjectReportInfoDto
{
    public int ProjectId { get; set; }
    public int ProjectAuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ReportDto> Reports { get; set; }
}
