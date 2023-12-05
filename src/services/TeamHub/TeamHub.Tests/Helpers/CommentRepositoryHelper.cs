using Moq;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class CommentRepositoryHelper
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;

        public CommentRepositoryHelper(Mock<ICommentRepository> commentRepositoryMock)
        {
            _commentRepositoryMock = commentRepositoryMock;
        }

        public void SetupAddAsync(
            Comment comment,
            CancellationToken cancellationToken,
            Comment result
        )
        {
            _commentRepositoryMock
                .Setup(x => x.AddAsync(comment, cancellationToken))
                .ReturnsAsync(result);
        }

        public void SetupSaveAsync(CancellationToken cancellationToken)
        {
            _commentRepositoryMock
                .Setup(x => x.SaveAsync(cancellationToken))
                .Returns(Task.CompletedTask);
        }

        public void SetupDelete(Comment comment)
        {
            _commentRepositoryMock.Setup(x => x.Delete(comment));
        }
    }
}
