using TeamHub.DAL.Models;

namespace TeamHub.DAL.Contracts.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    public Task<Project> GetProjectByIdAsync(int projectId);
}
