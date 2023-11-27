using AutoMapper;
using Shared.gRPC;
using Shared.gRPC.FullProjectResponse;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.gRPC
{
    public class FullProjectInfoService : IFullProjectInfoService
    {
        private readonly IProjectQueryService _projectService;
        private readonly ITeamMemberQueryService _teamMemberService;
        private readonly IMapper _mapper;

        public FullProjectInfoService(
            IProjectQueryService projectService,
            ITeamMemberQueryService teamMemberService,
            IMapper mapper
        )
        {
            _projectService = projectService;
            _teamMemberService = teamMemberService;
            _mapper = mapper;
        }

        public async Task<FullProjectInfoResponse> GetFullProjectInfoAsync(
            FullProjectInfoRequest fullProjectInfoRequest
        )
        {
            var project = await _projectService.GetExistingProjectAsync(
                fullProjectInfoRequest.ProjectId
            );
            await _teamMemberService.GetExistingTeamMemberAsync(
                fullProjectInfoRequest.UserId,
                fullProjectInfoRequest.ProjectId
            );

            var response = _mapper.Map<FullProjectInfoResponse>(project);

            return response;
        }
    }
}
