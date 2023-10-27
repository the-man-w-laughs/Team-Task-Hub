using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;
using Shared.Exceptions;

namespace ReportHub.BLL.services
{
    public class ProjectReportInfoService : IProjectReportInfoService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectReportInfoService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ReportDto>> GetAllProjectReportAsync(int projectId)
        {
            var userId = _httpContextAccessor.GetUserId();

            var project = await _projectReportInfoRepository.GetOneAsync(
                info => info.ProjectId == projectId
            );

            if (project == null || project.ProjectAuthorId != userId)
            {
                throw new NotFoundException("Project with id {projectId} was not found.");
            }

            var result = _mapper.Map<List<ReportDto>>(project.Reports);

            return result;
        }
    }
}
