using Shared.Repository.Sql;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Contracts.Repositories;

public interface ITaskHandlerRepository : IRepository<TaskHandler>
{
    Task<TaskHandler?> GetTaskHandlerAsync(
        int teamMemberId,
        int taskId,
        CancellationToken cancellationToken
    );
}
