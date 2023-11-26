using AutoMapper;
using Shared.gRPC;
using Shared.gRPC.FullProjectResponse;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.gRPC
{
    public class FullProjectInfoService : IFullProjectInfoService
    {
        private readonly IProjectService _projectService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IMapper _mapper;

        public FullProjectInfoService(
            IProjectService projectService,
            ITeamMemberService teamMemberService,
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
            await _teamMemberService.GetTeamMemberAsync(
                fullProjectInfoRequest.UserId,
                fullProjectInfoRequest.ProjectId
            );

            var project = await _projectService.GetProjectAsync(fullProjectInfoRequest.ProjectId);

            var response = _mapper.Map<FullProjectInfoResponse>(project);

            return response;
        }
    }
}
