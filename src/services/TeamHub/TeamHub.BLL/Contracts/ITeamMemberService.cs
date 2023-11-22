using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ITeamMemberService
    {
        Task<TeamMember> GetTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken
        );

        Task<TeamMember> AddTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken
        );

        Task<TeamMember> RemoveTeamMemberAsync(
            int userId,
            int projectId,
            CancellationToken cancellationToken
        );
    }
}
