using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ITeamMemberQueryService
    {
        Task<TeamMember> GetExistingTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken = default
        );
    }
}
