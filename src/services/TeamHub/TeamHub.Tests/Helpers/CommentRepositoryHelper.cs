using System.Linq.Expressions;
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

        public void SetupAddAsync(Comment result)
        {
            _commentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Comment>(), CancellationToken.None))
                .ReturnsAsync(result);
        }

        public void SetupSaveAsync()
        {
            _commentRepositoryMock
                .Setup(x => x.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask);
        }

        public void SetupDelete()
        {
            _commentRepositoryMock.Setup(x => x.Delete(It.IsAny<Comment>()));
        }

        public void SetupUpdate()
        {
            _commentRepositoryMock.Setup(x => x.Update(It.IsAny<Comment>()));
        }

        public void SetupGetAllAsync(int offset, int limit, List<Comment> comments)
        {
            _commentRepositoryMock
                .Setup(
                    x =>
                        x.GetAllAsync(
                            It.IsAny<Expression<Func<Comment, bool>>>(),
                            offset,
                            limit,
                            CancellationToken.None
                        )
                )
                .ReturnsAsync(comments);
        }
    }
}
