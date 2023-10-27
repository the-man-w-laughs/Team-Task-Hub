using ReportHub.BLL.Dtos;

namespace ReportHub.BLL.Contracts
{
    public interface IProjectReportInfoService
    {
        Task<List<ReportDto>> GetAllProjectReportAsync(int projectId);
    }
}
