using Shared.Repository.Sql;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Contracts.Repositories;

public interface ITaskModelRepository : IRepository<TaskModel>
{
    public Task<TaskModel> GetTaskByIdAsync(int taskId);
}
