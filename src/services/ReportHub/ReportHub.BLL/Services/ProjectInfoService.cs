using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;

namespace ReportHub.BLL.Services
{
    public class ProjectInfoService : IProjectInfoService
    {
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectInfoService(
            IProjectReportInfoRepository projectReportInfoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ProjectReportInfoDto>> GetAllUserProjectInfosAsync(
            int offset,
            int limit
        )
        {
            var userId = _httpContextAccessor.GetUserId();

            var projects = await _projectReportInfoRepository.GetAllAsync(
                project => project.ProjectAuthorId == userId,
                offset,
                limit
            );

            var result = _mapper.Map<List<ProjectReportInfoDto>>(projects);

            return result;
        }
    }
}
