using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Application.ResultPattern;
using Moq;

namespace Identity.Tests.Helpers
{
    public class UserServiceHelper
    {
        private readonly Mock<IUserService> _userServiceMock;

        public UserServiceHelper(Mock<IUserService> userServiceMock)
        {
            _userServiceMock = userServiceMock;
        }

        public void SetupAddUserAsync(Result<string> expectedResult)
        {
            _userServiceMock
                .Setup(service => service.AddUserAsync(It.IsAny<AppUserRegisterDto>()))
                .ReturnsAsync(expectedResult);
        }

        public void SetupGetAllUsersAsync(
            int offset,
            int limit,
            Result<List<AppUserDto>> expectedResult
        )
        {
            _userServiceMock
                .Setup(service => service.GetAllUsersAsync(offset, limit))
                .ReturnsAsync(expectedResult);
        }

        public void SetupGetUserByIdAsync(int userId, Result<AppUserDto> expectedResult)
        {
            _userServiceMock
                .Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedResult);
        }

        public void SetupDeleteUserByIdAsync(int userId, Result<AppUserDto> expectedResult)
        {
            _userServiceMock
                .Setup(service => service.DeleteUserByIdAsync(userId))
                .ReturnsAsync(expectedResult);
        }

        public void SetupDeleteUserByIdAsync(string email, Result<AppUserDto> expectedResult)
        {
            _userServiceMock
                .Setup(service => service.GetUserByEmailAsync(email))
                .ReturnsAsync(expectedResult);
        }
    }
}
