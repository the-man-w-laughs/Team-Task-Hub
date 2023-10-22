using Shared.Exceptions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            var project = await GetByIdAsync(projectId);

            if (project == null)
            {
                throw new NotFoundException($"Cannot find project with id {projectId}");
            }

            return project;
        }
    }
}
