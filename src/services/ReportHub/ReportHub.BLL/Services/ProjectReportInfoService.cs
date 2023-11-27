using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;
using Shared.Exceptions;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace ReportHub.BLL.Services
{
    public class ProjectReportInfoService : IProjectReportInfoService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProjectReportInfoService> _logger;

        public ProjectReportInfoService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ProjectReportInfoService> logger
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<ReportDto>> GetAllProjectReportsAsync(
            int projectId,
            int offset,
            int limit
        )
        {
            var userId = _httpContextAccessor.GetUserId();

            _logger.LogInformation(
                "Attempting to retrieve reports for project with ID {ProjectId} by user with ID {UserId}.",
                projectId,
                userId
            );

            var project = await _projectReportInfoRepository.GetOneAsync(
                info => info.ProjectId == projectId
            );

            if (project == null || project.ProjectAuthorId != userId)
            {
                _logger.LogInformation(
                    "Project with ID {ProjectId} not found or user with ID {UserId} is not the project author.",
                    projectId,
                    userId
                );

                throw new NotFoundException($"Project with ID {projectId} was not found.");
            }

            var reports = project.Reports.Skip(offset).Take(limit).ToList();
            var result = _mapper.Map<List<ReportDto>>(reports);

            _logger.LogInformation(
                "Reports retrieved for project with ID {ProjectId} by user with ID {UserId}.",
                projectId,
                userId
            );

            return result;
        }
    }
}
