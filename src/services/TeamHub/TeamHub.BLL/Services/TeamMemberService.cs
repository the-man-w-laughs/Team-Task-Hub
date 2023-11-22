using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        public async Task<TeamMember> GetTeamMemberAsync(
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

        public async Task<TeamMember> AddTeamMemberAsync(
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

            if (teamMember != null)
            {
                throw new ForbiddenException(
                    $"User with id {userId} is already a team member of project with id {projectId}."
                );
            }

            // create new team member
            var teamMemberToAdd = new TeamMember()
            {
                ProjectId = projectId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            var addedTeamMember = await _teamMemberRepository.AddAsync(
                teamMemberToAdd,
                cancellationToken
            );
            await _teamMemberRepository.SaveAsync(cancellationToken);

            return addedTeamMember;
        }

        public async Task<TeamMember> RemoveTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken
        )
        {
            var teamMemberToDelete = await GetTeamMemberAsync(userId, projectId, cancellationToken);

            _teamMemberRepository.Delete(teamMemberToDelete);
            await _teamMemberRepository.SaveAsync(cancellationToken);

            return teamMemberToDelete;
        }
    }
}
