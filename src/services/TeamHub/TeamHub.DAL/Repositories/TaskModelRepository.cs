using Shared.Repository.Sql;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class TaskModelRepository : Repository<TeamHubDbContext, TaskModel>, ITaskModelRepository
    {
        public TaskModelRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }
    }
}
