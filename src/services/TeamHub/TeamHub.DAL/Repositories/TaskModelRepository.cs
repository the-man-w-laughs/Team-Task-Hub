using Shared.Exceptions;
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

        public async Task<TaskModel> GetTaskByIdAsync(
            int taskId,
            CancellationToken cancellationToken = default
        )
        {
            var task = await GetByIdAsync(taskId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException($"Task with id {taskId} was not found.");
            }

            return task;
        }
    }
}
