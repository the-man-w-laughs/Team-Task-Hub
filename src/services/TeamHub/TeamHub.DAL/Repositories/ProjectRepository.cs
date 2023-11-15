using Shared.Exceptions;
using Shared.Repository.Sql;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class ProjectRepository : Repository<TeamHubDbContext, Project>, IProjectRepository
    {
        public ProjectRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public async Task<Project> GetProjectByIdAsync(
            int projectId,
            CancellationToken cancellationToken = default
        )
        {
            var project = await GetByIdAsync(projectId, cancellationToken);

            if (project == null)
            {
                throw new NotFoundException($"Cannot find project with id {projectId}");
            }

            return project;
        }
    }
}
