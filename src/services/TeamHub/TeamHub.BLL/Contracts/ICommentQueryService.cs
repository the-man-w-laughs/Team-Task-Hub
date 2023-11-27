using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ICommentQueryService
    {
        Task<Comment> GetExistingCommentAsync(int commentId, CancellationToken cancellationToken);
    }
}
