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

        public void SetupGetExistingCommentAsync(int commentId, Comment result)
        {
            _commentQueryServiceMock
                .Setup(x => x.GetExistingCommentAsync(commentId, CancellationToken.None))
                .ReturnsAsync(result);
        }

        public void SetupGetExistingCommentAsync(int commentId, Exception exceptionToThrow)
        {
            _commentQueryServiceMock
                .Setup(x => x.GetExistingCommentAsync(commentId, CancellationToken.None))
                .ThrowsAsync(exceptionToThrow);
        }
    }
}
