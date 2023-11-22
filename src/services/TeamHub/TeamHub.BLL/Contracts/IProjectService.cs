using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface IProjectService
    {
        Task<Project> GetProjectAsync(int projectId, CancellationToken cancellationToken);
    }
}
