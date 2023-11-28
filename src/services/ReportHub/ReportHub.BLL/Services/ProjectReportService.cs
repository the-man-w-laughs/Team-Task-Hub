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
using Shared.gRPC;
using Microsoft.Extensions.Logging;

namespace ReportHub.BLL.Services
{
    public class ProjectReportService : IProjectReportService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMinioRepository _minioRepository;
        private readonly IFullProjectInfoService _fullProjectInfoService;
        private readonly ILogger<ProjectReportService> _logger;

        public ProjectReportService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMinioRepository minioRepository,
            IFullProjectInfoService fullProjectInfoService,
            ILogger<ProjectReportService> logger
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _minioRepository = minioRepository;
            _fullProjectInfoService = fullProjectInfoService;
            _logger = logger;
        }

        public async Task<FileStreamResult> GenerateProjectReportAsync(int projectId)
        {
            var userId = _httpContextAccessor.GetUserId();

            _logger.LogInformation(
                "User with ID {UserId} is generating a report for project with ID {ProjectId}.",
                userId,
                projectId
            );

            var request = new FullProjectInfoRequest() { ProjectId = projectId, UserId = userId };
            var fullProjectResponseDto = await _fullProjectInfoService.GetFullProjectInfoAsync(
                request
            );

            if (fullProjectResponseDto == null)
            {
                throw new NotFoundException(
                    $"Cannot fetch full project data for project with ID {projectId}."
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
                _logger.LogInformation(
                    "Creating new project report info for project with ID {ProjectId}.",
                    projectId
                );

                project = new ProjectReportInfo(projectId, userId);
                await _projectReportInfoRepository.CreateAsync(project);
            }

            var latestReportInfo = new Report(fileName);
            project.Reports.Add(latestReportInfo);
            await _projectReportInfoRepository.UpdateAsync(project);

            var contentType = "text/plain";
            contentStream.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "Report generated successfully for project with ID {ProjectId}. File: {FileName}.",
                projectId,
                fileName
            );

            return new FileStreamResult(contentStream, contentType) { FileDownloadName = fileName };
        }

        public async Task<FileStreamResult> GetReportByNameAsync(string path)
        {
            var userId = _httpContextAccessor.GetUserId();
            _logger.LogInformation(
                "User with ID {UserId} is attempting to retrieve report with filename {FileName}.",
                userId,
                path
            );

            var project = await _projectReportInfoRepository.GetOneAsync(
                projectReportInfo => projectReportInfo.Reports.Any(report => report.Path == path)
            );

            if (project == null || project.ProjectAuthorId != userId)
            {
                throw new NotFoundException($"Project report with filename {path} was not found.");
            }

            var result = await _minioRepository.GetFileFromMinioAsync(path);
            var contentType = "text/plain";

            _logger.LogInformation(
                "Report with filename {FileName} successfully retrieved for user with ID {UserId}.",
                path,
                userId
            );

            return new FileStreamResult(result, contentType) { FileDownloadName = path };
        }

        public async Task<string> DeleteReportByNameAsync(string path)
        {
            var userId = _httpContextAccessor.GetUserId();
            _logger.LogInformation(
                "User with ID {UserId} is attempting to delete report with filename {FileName}.",
                userId,
                path
            );

            var project = await _projectReportInfoRepository.GetOneAsync(
                projectReportInfo => projectReportInfo.Reports.Any(report => report.Path == path)
            );

            if (project == null || project.ProjectAuthorId != userId)
            {
                throw new NotFoundException($"Project report with filename {path} was not found.");
            }

            await _minioRepository.DeleteFileFromMinioAsync(path);
            var reportToRemove = project.Reports.FirstOrDefault(report => report.Path == path);

            if (reportToRemove != null)
            {
                project.Reports.Remove(reportToRemove);
                await _projectReportInfoRepository.UpdateAsync(project);

                _logger.LogInformation(
                    "Report with filename {FileName} successfully deleted for user with ID {UserId}.",
                    path,
                    userId
                );

                return path;
            }
            else
            {
                throw new NotFoundException($"Project report with filename {path} was not found.");
            }
        }
    }
}
