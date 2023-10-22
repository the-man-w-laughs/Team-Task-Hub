using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class TeamMemberRepository : Repository<TeamMember>, ITeamMemberRepository
    {
        public TeamMemberRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public async Task<TeamMember> GetTeamMemberAsync(int userId, int projectId)
        {
            var teamMember = await TeamHubDbContext.TeamMembers.FirstOrDefaultAsync(
                teamMember => teamMember.UserId == userId && teamMember.ProjectId == projectId
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
