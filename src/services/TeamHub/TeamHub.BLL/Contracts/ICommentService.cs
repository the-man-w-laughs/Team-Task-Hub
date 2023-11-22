using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ICommentService
    {
        Task<Comment> GetCommentAsync(int commentId, CancellationToken cancellationToken);
    }
}
