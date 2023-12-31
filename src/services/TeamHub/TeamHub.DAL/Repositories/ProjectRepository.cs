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
    }
}
