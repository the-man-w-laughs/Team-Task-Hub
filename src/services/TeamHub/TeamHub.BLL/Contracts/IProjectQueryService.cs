using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface IProjectQueryService
    {
        Task<Project> GetExistingProjectAsync(
            int projectId,
            CancellationToken cancellationToken = default
        );
    }
}
