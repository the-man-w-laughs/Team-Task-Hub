using TeamHub.DAL.Models;

namespace TeamHub.DAL.Contracts.Repositories;

public interface ITeamMemberRepository : IRepository<TeamMember>
{
    public Task<TeamMember> GetTeamMemberAsync(int userId, int projectId);
}
