using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ITaskQueryService
    {
        Task<TaskModel> GetExistingTaskAsync(int taskId, CancellationToken cancellationToken);
    }
}
