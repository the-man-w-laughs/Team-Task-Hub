using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;
using TeamHub.BLL.Dtos;

namespace ReportHub.BLL.Contracts
{
    public interface IProjectReportInfoService
    {
        Task<List<ProjectReportInfoDto>> GetAllUsersProjectReportInfo();
        Task<Report> GetLatestProjectReportById(int projectId);
        Task<FullProjectResponseDto> GetProjectsDataById(int projectId);
    }
}
