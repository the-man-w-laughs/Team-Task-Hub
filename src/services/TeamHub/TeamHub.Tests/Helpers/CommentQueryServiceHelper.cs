using Moq;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class CommentQueryServiceHelper
    {
        private readonly Mock<ICommentQueryService> _commentQueryServiceMock;

        public CommentQueryServiceHelper(Mock<ICommentQueryService> commentQueryServiceMock)
        {
            _commentQueryServiceMock = commentQueryServiceMock;
        }

        public void SetupGetExistingCommentAsync(
            int commentId,
            CancellationToken cancellationToken,
            Comment result
        )
        {
            _commentQueryServiceMock
                .Setup(x => x.GetExistingCommentAsync(commentId, cancellationToken))
                .ReturnsAsync(result);
        }

        public void SetupGetExistingCommentAsync(
            int commentId,
            CancellationToken cancellationToken,
            Exception exceptionToThrow
        )
        {
            _commentQueryServiceMock
                .Setup(x => x.GetExistingCommentAsync(commentId, cancellationToken))
                .ThrowsAsync(exceptionToThrow);
        }
    }
}
