using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;

namespace ReportHub.BLL.Contracts
{
    public interface IProjectReportInfoService
    {
        Task<List<ReportDto>> GetAllProjectReportsAsync(int projectId, int offset, int limit);
    }
}
