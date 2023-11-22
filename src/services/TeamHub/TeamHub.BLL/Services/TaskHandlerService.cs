using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class TaskHandlerService : ITaskHandlerService
    {
        private readonly ITaskHandlerRepository _taskHandlerRepository;

        public TaskHandlerService(ITaskHandlerRepository taskHandlerRepository)
        {
            _taskHandlerRepository = taskHandlerRepository;
        }

        public async Task<TaskHandler> AddTaskHandlerAsync(
            TeamMember teamMember,
            int taskId,
            CancellationToken cancellationToken
        )
        {
            // Check if the task handler already exists.
            var taskHandler = await _taskHandlerRepository.GetTaskHandlerAsync(
                teamMember.Id,
                taskId,
                cancellationToken
            );

            if (taskHandler != null)
            {
                throw new WrongActionException(
                    $"User with id {teamMember.UserId} is already assigned to the task with Id {taskId}."
                );
            }

            // Create a new TaskHandler entity and add it to the repository.
            var taskHandlerToAdd = new TaskHandler()
            {
                TaskId = taskId,
                TeamMemberId = teamMember.Id,
                CreatedAt = DateTime.Now
            };

            var addedTaskHandler = await _taskHandlerRepository.AddAsync(
                taskHandlerToAdd,
                cancellationToken
            );
            await _taskHandlerRepository.SaveAsync(cancellationToken);

            return addedTaskHandler;
        }

        public async Task<TaskHandler> RemoveTaskHandlerAsync(
            TeamMember teamMember,
            int taskId,
            CancellationToken cancellationToken
        )
        {
            // Check if the task handler already exists.
            var taskHandler = await _taskHandlerRepository.GetTaskHandlerAsync(
                teamMember.Id,
                taskId,
                cancellationToken
            );

            if (taskHandler == null)
            {
                throw new WrongActionException(
                    $"User with id {teamMember.UserId} is not assigned to the task with Id {taskId}."
                );
            }

            _taskHandlerRepository.Delete(taskHandler);
            await _taskHandlerRepository.SaveAsync(cancellationToken);

            return taskHandler;
        }
    }
}
