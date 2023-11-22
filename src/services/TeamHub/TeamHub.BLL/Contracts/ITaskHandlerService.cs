using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ITaskHandlerService
    {
        Task<TaskHandler> AddTaskHandlerAsync(
            TeamMember teamMember,
            int taskId,
            CancellationToken cancellationToken
        );
        Task<TaskHandler> RemoveTaskHandlerAsync(
            TeamMember teamMember,
            int taskId,
            CancellationToken cancellationToken
        );
    }
}
