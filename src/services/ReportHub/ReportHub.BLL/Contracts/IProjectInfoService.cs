using ReportHub.BLL.Dtos;

namespace ReportHub.BLL.Contracts
{
    public interface IProjectInfoService
    {
        Task<List<ProjectReportInfoDto>> GetAllUserProjectInfosAsync(int offset, int limit);
    }
}
