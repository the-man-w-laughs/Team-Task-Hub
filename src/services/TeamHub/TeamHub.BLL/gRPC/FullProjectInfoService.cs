using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.gRPC;
using Shared.gRPC.FullProjectResponse;
using TeamHub.DAL.Contracts.Repositories;
using Shared.Extensions;

namespace TeamHub.BLL.gRPC
{
    public class FullProjectInfoService : IFullProjectInfoService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IMapper _mapper;

        public FullProjectInfoService(
            IProjectRepository projectRepository,
            ITeamMemberRepository teamMemberRepository,
            IMapper mapper
        )
        {
            _projectRepository = projectRepository;
            _teamMemberRepository = teamMemberRepository;
            _mapper = mapper;
        }

        public async Task<FullProjectInfoResponse> GetProjectTaskAsync(
            FullProjectInfoRequest fullProjectInfoRequest
        )
        {
            var project = await _projectRepository.GetProjectByIdAsync(
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
