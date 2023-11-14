using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.DAL.Models;
using System.Text;
using Shared.Exceptions;
using ReportHub.BLL.Extensions;
using ReportHub.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.gRPC.FullProjectResponse;
using Shared.gRPC;

namespace ReportHub.BLL.services
{
    public class ProjectReportService : IProjectReportService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMinioRepository _minioRepository;
        private readonly IFullProjectInfoService _fullProjectInfoService;

        public ProjectReportService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMinioRepository minioRepository,
            IFullProjectInfoService fullProjectInfoService
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _minioRepository = minioRepository;
            _fullProjectInfoService = fullProjectInfoService;
        }

        public async Task<FileStreamResult> GetLatestProjectReportAsync(int projectId)
        {
            var userId = _httpContextAccessor.GetUserId();

            var request = new FullProjectInfoRequest() { ProjectId = projectId, UserId = userId };

            var fullProjectResponseDto = await _fullProjectInfoService.GetFullProjectInfoAsync(
                request
            );

            if (fullProjectResponseDto == null)
            {
                throw new NotFoundException(
                    $"Cannot fetch full project data for with id {projectId}."
                );
            }

            var reportContent = fullProjectResponseDto.ToReport();

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));

            var fileName = await _minioRepository.UploadReportAsync(contentStream);

            var project = await _projectReportInfoRepository.GetOneAsync(
                info => info.ProjectId == projectId
            );

            if (project == null)
            {
                project = new ProjectReportInfo()
                {
                    ProjectId = projectId,
                    ProjectAuthorId = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Reports = new List<Report>()
                };

                await _projectReportInfoRepository.CreateAsync(project);
            }

            var latestReportInfo = new Report() { Path = fileName, GeneratedAt = DateTime.Now };

            project.Reports.Add(latestReportInfo);

            await _projectReportInfoRepository.UpdateAsync(project);

            var contentType = "text/plain";

            contentStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(contentStream, contentType) { FileDownloadName = fileName };
        }

        public async Task<FileStreamResult> GetReportByNameAsync(string path)
        {
            var userId = _httpContextAccessor.GetUserId();

            var project = await _projectReportInfoRepository.GetOneAsync(
                projectReportInfo => projectReportInfo.Reports.Any(report => report.Path == path)
            );

            if (project == null || project.ProjectAuthorId != userId)
            {
                throw new NotFoundException($"Project report with filename {path} was not found");
            }

            var result = await _minioRepository.GetFileFromMinioAsync(path);

            var contentType = "text/plain";

            return new FileStreamResult(result, contentType) { FileDownloadName = path };
        }

        public async Task<string> DeleteReportByNameAsync(string path)
        {
            var userId = _httpContextAccessor.GetUserId();

            var project = await _projectReportInfoRepository.GetOneAsync(
                projectReportInfo => projectReportInfo.Reports.Any(report => report.Path == path)
            );

            if (project == null || project.ProjectAuthorId != userId)
            {
                throw new NotFoundException($"Project report with filename {path} was not found");
            }

            await _minioRepository.DeleteFileFromMinioAsync(path);

            var reportToRemove = project.Reports.FirstOrDefault(report => report.Path == path);

            if (reportToRemove != null)
            {
                project.Reports.Remove(reportToRemove);
                await _projectReportInfoRepository.UpdateAsync(project);

                return path;
            }
            else
            {
                throw new NotFoundException($"Project report with filename {path} was not found");
            }
        }

        public Report? GetLatestReport(ProjectReportInfo projectReportInfo)
        {
            var latestReport = projectReportInfo.Reports
                .Where(report => report.GeneratedAt > projectReportInfo.UpdatedAt)
                .OrderByDescending(report => report.GeneratedAt)
                .FirstOrDefault();

            return latestReport;
        }
    }
}
