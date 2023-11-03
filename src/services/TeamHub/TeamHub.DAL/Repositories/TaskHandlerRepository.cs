using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class TaskHandlerRepository : Repository<TaskHandler>, ITaskHandlerRepository
    {
        public TaskHandlerRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public Task<TaskHandler?> GetTaskHandlerAsync(int teamMemberId, int taskId)
        {
            return GetAsync(
                handler => handler.TasksId == taskId && handler.TeamMemberId == teamMemberId
            );
        }
    }
}
