using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.DAL.Models;
using TeamHub.BLL.Dtos;
using System.Text;
using Shared.Exceptions;
using ReportHub.BLL.Extensions;
using ReportHub.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ReportHub.BLL.services
{
    public class ProjectReportService : IProjectReportService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMinioRepository _minioRepository;

        public ProjectReportService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IMinioRepository minioRepository
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _minioRepository = minioRepository;
        }

        public async Task<FileStreamResult> GetLatestProjectReportAsync(int projectId)
        {
            var userId = _httpContextAccessor.GetUserId();

            var projectReportInfo = await _projectReportInfoRepository.GetOneAsync(
                projectReportInfo => projectReportInfo.ProjectId == projectId
            );

            var latestReport = GetLatestReport(projectReportInfo);

            // if (latestReport != null && latestReport.GeneratedAt > projectReportInfo.UpdatedAt)
            // {
            //     return latestReport;
            // }

            FullProjectResponseDto fullProjectResponseDto = await GetFullProjectDataAsync(
                projectId
            );

            if (fullProjectResponseDto == null)
            {
                throw new NotFoundException(
                    $"Cannot fetch full project data for with id {projectId}."
                );
            }
            string reportContent = fullProjectResponseDto.ToReport();

            Stream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));

            var fileName = await _minioRepository.UploadReportAsync(contentStream);

            var project = await _projectReportInfoRepository.GetOneAsync(
                info => info.ProjectId == projectId
            );

            var latestReportInfo = new Report() { Path = fileName, GeneratedAt = DateTime.Now };

            project.Reports.Add(latestReportInfo);

            await _projectReportInfoRepository.UpdateAsync(project);

            string contentType = "text/plain";

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

            string contentType = "text/plain";

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

        private async Task<FullProjectResponseDto> GetFullProjectDataAsync(int projectId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Projects/{projectId}/full");

            var headers = _httpContextAccessor.HttpContext.Request.Headers;

            if (headers.TryGetValue("Authorization", out var authorizationHeaderValues))
            {
                var authorizationHeaderValue = authorizationHeaderValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(authorizationHeaderValue))
                {
                    request.Headers.Add("Authorization", authorizationHeaderValue);
                }
            }

            var teamHubClient = _httpClientFactory.CreateClient("TeamHubClient");

            HttpResponseMessage response = await teamHubClient.SendAsync(request);

            FullProjectResponseDto fullProjectResponseDto =
                await response.Content.ReadAsAsync<FullProjectResponseDto>();

            return fullProjectResponseDto;
        }
    }
}
