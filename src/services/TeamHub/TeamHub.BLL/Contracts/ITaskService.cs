using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ITaskService
    {
        Task<TaskModel> GetTaskAsync(int taskId, CancellationToken cancellationToken);
    }
}
