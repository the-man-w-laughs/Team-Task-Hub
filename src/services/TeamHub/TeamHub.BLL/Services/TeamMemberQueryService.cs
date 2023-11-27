using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class TeamMemberQueryService : ITeamMemberQueryService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberQueryService(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        public async Task<TeamMember> GetExistingTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken
        )
        {
            var teamMember = await _teamMemberRepository.GetTeamMemberAsync(
                userId,
                projectId,
                cancellationToken
            );

            if (teamMember == null)
            {
                throw new ForbiddenException(
                    $"User with id {userId} doesn't have access to project with id {projectId}."
                );
            }

            return teamMember;
        }
    }
}
