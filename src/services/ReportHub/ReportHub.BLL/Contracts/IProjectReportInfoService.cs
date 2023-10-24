using ReportHub.BLL.Dtos;

namespace ReportHub.BLL.Contracts
{
    public interface IProjectReportInfoService
    {
        Task<List<ProjectReportInfoDto>> GetAllUsersProjectReportInfo();
    }
}
