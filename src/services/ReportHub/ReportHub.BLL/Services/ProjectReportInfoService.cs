using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;
using TeamHub.BLL.Dtos;
using MongoDB.Bson.IO;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace ReportHub.BLL.services
{
    public class ProjectReportInfoService : IProjectReportInfoService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProjectReportInfoService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ProjectReportInfoDto>> GetAllUsersProjectReportInfo()
        {
            var userId = _httpContextAccessor.GetUserId();

            var result = await _projectReportInfoRepository.GetAllAsync(
                projectReportInfo => projectReportInfo.ProjectAuthorId == userId
            );

            return _mapper.Map<List<ProjectReportInfoDto>>(result);
        }

        public async Task<FullProjectResponseDto> GetProjectsDataById(int projectId)
        {
            var userId = _httpContextAccessor.GetUserId();

            var projectReportInfo = await _projectReportInfoRepository.GetOneAsync(
                projectReportInfo => projectReportInfo.ProjectId == projectId
            );

            FullProjectResponseDto fullProjectResponseDto = await GetFullProjectData(projectId);

            return fullProjectResponseDto;
        }

        public async Task<Report> GetLatestProjectReportById(int projectId)
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

            FullProjectResponseDto fullProjectResponseDto = await GetFullProjectData(projectId);

            string reportContent = JsonSerializer.Serialize(fullProjectResponseDto);

            using (var content = new MultipartFormDataContent())
            {
                var reportStream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));

                var fileContent = new StreamContent(reportStream);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(
                    "form-data"
                )
                {
                    Name = "\"file\"",
                    FileName = "\"report.txt\""
                };

                content.Add(fileContent);

                var nginxClient = _httpClientFactory.CreateClient("NginxClient");

                var request = new HttpRequestMessage(HttpMethod.Post, "upload")
                {
                    Content = content
                };

                var response = await nginxClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                { /* Handle success */
                }
                else
                { /* Handle error */
                }
            }

            return latestReport;
        }

        public Report? GetLatestReport(ProjectReportInfo projectReportInfo)
        {
            var latestReport = projectReportInfo.Reports
                .Where(report => report.GeneratedAt > projectReportInfo.UpdatedAt)
                .OrderByDescending(report => report.GeneratedAt)
                .FirstOrDefault();

            return latestReport;
        }

        private async Task<FullProjectResponseDto> GetFullProjectData(int projectId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Projects/{projectId}/full");

            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value.ToString());
            }

            var teamHubClient = _httpClientFactory.CreateClient("TeamHubClient");

            HttpResponseMessage response = await teamHubClient.SendAsync(request);

            FullProjectResponseDto fullProjectResponseDto =
                await response.Content.ReadAsAsync<FullProjectResponseDto>();
            return fullProjectResponseDto;
        }
    }
}
