using Shared.Repository.Sql;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Contracts.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    public Task<Comment> GetCommentByIdAsync(int commentId);
}
