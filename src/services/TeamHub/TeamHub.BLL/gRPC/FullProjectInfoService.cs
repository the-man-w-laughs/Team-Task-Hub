using AutoMapper;
using Shared.gRPC;
using Shared.gRPC.FullProjectResponse;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.gRPC
{
    public class FullProjectInfoService : IFullProjectInfoService
    {
        private readonly IProjectQueryService _projectService;
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IMapper _mapper;

        public FullProjectInfoService(
            IProjectQueryService projectService,
            ITeamMemberRepository teamMemberRepository,
            IMapper mapper
        )
        {
            _projectService = projectService;
            _teamMemberRepository = teamMemberRepository;
            _mapper = mapper;
        }

        public async Task<FullProjectInfoResponse> GetFullProjectInfoAsync(
            FullProjectInfoRequest fullProjectInfoRequest
        )
        {
            var project = await _projectService.GetExistingProjectAsync(
                fullProjectInfoRequest.ProjectId
            );
            await _teamMemberRepository.GetTeamMemberAsync(
                fullProjectInfoRequest.UserId,
                fullProjectInfoRequest.ProjectId
            );

            var response = _mapper.Map<FullProjectInfoResponse>(project);

            return response;
        }
    }
}
