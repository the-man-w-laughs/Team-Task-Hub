using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class CommentQueryService : ICommentQueryService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentQueryService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment> GetExistingCommentAsync(
            int commentId,
            CancellationToken cancellationToken
        )
        {
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);

            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {commentId} not found.");
            }

            return comment;
        }
    }
}
