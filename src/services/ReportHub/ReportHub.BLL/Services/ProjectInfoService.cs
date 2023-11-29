using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace ReportHub.BLL.Services
{
    public class ProjectInfoService : IProjectInfoService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProjectInfoService> _logger;

        public ProjectInfoService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ProjectInfoService> logger
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<ProjectReportInfoDto>> GetAllUserProjectInfosAsync(
            int offset,
            int limit
        )
        {
            var userId = _httpContextAccessor.GetUserId();

            _logger.LogInformation(
                "User with ID {UserId} is attempting to retrieve their projects with offset {Offset} and limit {Limit}.",
                userId,
                offset,
                limit
            );

            var projects = await _projectReportInfoRepository.GetAllAsync(
                offset,
                limit,
                project => project.ProjectAuthorId == userId
            );

            var result = _mapper.Map<List<ProjectReportInfoDto>>(projects);

            _logger.LogInformation(
                "Returning the list of projects for user with ID {UserId}.",
                userId
            );

            return result;
        }
    }
}
