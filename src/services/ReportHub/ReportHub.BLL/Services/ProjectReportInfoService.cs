using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using ReportHub.BLL.Contracts;
using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;

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

        public async Task<List<ProjectReportInfoDto>> GetAllUsersProjectReportInfo()
        {
            var userId = _httpContextAccessor.GetUserId();

            var result = await _projectReportInfoRepository.GetAllAsync(
                projectReportInfo => projectReportInfo.ProjectAuthorId == userId
            );

            return _mapper.Map<List<ProjectReportInfoDto>>(result);
        }
    }
}
