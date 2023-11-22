using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment> GetCommentAsync(
            int commentId,
            CancellationToken cancellationToken
        )
        {
            var comment = await _commentRepository.GetCommentByIdAsync(
                commentId,
                cancellationToken
            );

            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {commentId} not found.");
            }

            return comment;
        }
    }
}
