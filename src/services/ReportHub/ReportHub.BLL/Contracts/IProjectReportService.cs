using Microsoft.AspNetCore.Mvc;

namespace ReportHub.BLL.Contracts
{
    public interface IProjectReportService
    {
        Task<FileStreamResult> GetLatestProjectReportAsync(int projectId);
        Task<FileStreamResult> GetReportByNameAsync(string path);
        Task<string> DeleteReportByNameAsync(string path);
    }
}
