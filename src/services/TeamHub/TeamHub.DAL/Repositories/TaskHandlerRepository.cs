using Shared.Repository.Sql;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class TaskHandlerRepository
        : Repository<TeamHubDbContext, TaskHandler>,
            ITaskHandlerRepository
    {
        public TaskHandlerRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public Task<TaskHandler?> GetTaskHandlerAsync(
            int teamMemberId,
            int taskId,
            CancellationToken cancellationToken = default
        )
        {
            return GetAsync(
                handler => handler.TaskId == taskId && handler.TeamMemberId == teamMemberId,
                cancellationToken
            );
        }
    }
}
