using Shared.Exceptions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            var comment = await GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFoundException($"Cannot find comment with id {commentId}");
            }

            return comment;
        }
    }
}
