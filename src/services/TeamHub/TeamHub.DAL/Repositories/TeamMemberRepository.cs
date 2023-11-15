using Shared.Repository.Sql;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class TeamMemberRepository
        : Repository<TeamHubDbContext, TeamMember>,
            ITeamMemberRepository
    {
        public TeamMemberRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public async Task<TeamMember?> GetTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken = default
        )
        {
            var teamMember = await GetAsync(
                teamMember => teamMember.UserId == userId && teamMember.ProjectId == projectId,
                cancellationToken
            );

            return teamMember;
        }
    }
}
