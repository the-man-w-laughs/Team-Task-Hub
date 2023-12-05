using Moq;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class UserQueryServiceHelper
    {
        private readonly Mock<IUserQueryService> _userQueryServiceMock;

        public UserQueryServiceHelper(Mock<IUserQueryService> userQueryServiceMock)
        {
            _userQueryServiceMock = userQueryServiceMock;
        }

        public void SetupGetExistingUserAsync(
            int userId,
            CancellationToken cancellationToken,
            User result
        )
        {
            _userQueryServiceMock
                .Setup(x => x.GetExistingUserAsync(userId, cancellationToken))
                .ReturnsAsync(result);
        }

        public void SetupGetExistingUserAsync(
            int userId,
            CancellationToken cancellationToken,
            Exception exceptionToThrow
        )
        {
            _userQueryServiceMock
                .Setup(x => x.GetExistingUserAsync(userId, cancellationToken))
                .ThrowsAsync(exceptionToThrow);
        }
    }
}
