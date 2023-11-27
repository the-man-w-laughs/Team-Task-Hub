using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class TaskQueryService : ITaskQueryService
    {
        private readonly ITaskModelRepository _taskRepository;

        public TaskQueryService(ITaskModelRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskModel> GetExistingTaskAsync(
            int taskId,
            CancellationToken cancellationToken
        )
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException($"Task with id {taskId} was not found.");
            }

            return task;
        }
    }
}
