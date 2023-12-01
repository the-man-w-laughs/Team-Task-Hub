using Bogus;
using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Application.ResultPattern.Results;
using Identity.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly UsersController _usersController;
        private readonly Faker<AppUserRegisterDto> _appUserRegisterDto;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _usersController = new UsersController(_userService.Object);

            _appUserRegisterDto = new Faker<AppUserRegisterDto>()
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password());
        }

        [Fact]
        public async Task CreateNewUserAsync_SuccessfullyCreatedUser_ReturnsOk()
        {
            // Arrange
            var appUserRegisterDto = _appUserRegisterDto.Generate();
            var expectedResult = new SuccessResult<string>(string.Empty);

            _userService
                .Setup(service => service.AddUserAsync(appUserRegisterDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _usersController.CreateNewUserAsync(appUserRegisterDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateNewUserAsync_FailedToCreateUser_ReturnsbadRequest()
        {
            // Arrange
            var appUserRegisterDto = _appUserRegisterDto.Generate();
            var expectedResult = new InvalidResult<string>(string.Empty);

            _userService
                .Setup(service => service.AddUserAsync(appUserRegisterDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _usersController.CreateNewUserAsync(appUserRegisterDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
