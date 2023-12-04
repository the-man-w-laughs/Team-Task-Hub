using Bogus;
using FluentAssertions;
using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Application.ResultPattern.Results;
using Identity.Tests.Helpers;
using Identity.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly UsersController _usersController;
        private readonly UserServiceHelper _userServiceHelper;
        private readonly Faker<AppUserRegisterDto> _appUserRegisterDto;
        private readonly Faker<AppUserDto> _appUserDto;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _usersController = new UsersController(_userService.Object);
            _userServiceHelper = new UserServiceHelper(_userService);

            _appUserRegisterDto = new Faker<AppUserRegisterDto>()
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password());

            _appUserDto = new Faker<AppUserDto>()
                .RuleFor(u => u.Id, f => f.Random.Number(max: int.MaxValue))
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.IsOnline, f => f.Random.Bool());
        }

        [Fact]
        public async Task CreateNewUserAsync_UserDoesNotExist_ReturnsOk()
        {
            // Arrange
            var appUserRegisterDto = _appUserRegisterDto.Generate();
            var expectedResult = new SuccessResult<string>(string.Empty);
            _userServiceHelper.SetupAddUserAsync(expectedResult);

            // Act
            var result = await _usersController.CreateNewUserAsync(appUserRegisterDto);

            // Assert
            result.Should().BeAssignableTo<OkObjectResult>();
            _userService.Verify(repo => repo.AddUserAsync(appUserRegisterDto), Times.Once);
        }

        [Fact]
        public async Task CreateNewUserAsync_UserExists_ReturnBadRequest()
        {
            // Arrange
            var appUserRegisterDto = _appUserRegisterDto.Generate();
            var expectedResult = new InvalidResult<string>(string.Empty);
            _userServiceHelper.SetupAddUserAsync(expectedResult);

            // Act
            var result = await _usersController.CreateNewUserAsync(appUserRegisterDto);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _userService.Verify(repo => repo.AddUserAsync(appUserRegisterDto), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersAsync_FullRepository_ReturnsOk()
        {
            // Arrange
            var userCount = 10;
            var offset = 0;
            var limit = 5;
            var users = _appUserDto.Generate(userCount);
            var expectedResult = new SuccessResult<List<AppUserDto>>(
                users.Skip(offset).Take(limit).ToList()
            );
            _userServiceHelper.SetupGetAllUsersAsync(offset, limit, expectedResult);

            // Act
            var result = await _usersController.GetAllUsersAsync(offset, limit);

            // Assert
            result
                .Should()
                .BeAssignableTo<OkObjectResult>()
                .Which.Value.Should()
                .BeEquivalentTo(expectedResult.Value);
            _userService.Verify(repo => repo.GetAllUsersAsync(offset, limit), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsOk()
        {
            // Arrange
            var user = _appUserDto.Generate();
            var expectedResult = new SuccessResult<AppUserDto>(user);
            _userServiceHelper.SetupGetUserByIdAsync(user.Id, expectedResult);

            // Act
            var result = await _usersController.GetUserByIdAsync(user.Id);

            // Assert
            result
                .Should()
                .BeAssignableTo<OkObjectResult>()
                .Which.Value.Should()
                .BeEquivalentTo(user);
            _userService.Verify(repo => repo.GetUserByIdAsync(user.Id), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserDoesNotExist_ReturnsOk()
        {
            // Arrange
            var user = _appUserDto.Generate();
            var expectedResult = new InvalidResult<AppUserDto>(string.Empty);
            _userServiceHelper.SetupGetUserByIdAsync(user.Id, expectedResult);

            // Act
            var result = await _usersController.GetUserByIdAsync(user.Id);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _userService.Verify(repo => repo.GetUserByIdAsync(user.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdUserAsync_UserExists_ReturnsOk()
        {
            // Arrange
            var appUserDto = _appUserDto.Generate();
            var expectedResult = new SuccessResult<AppUserDto>(appUserDto);
            _userServiceHelper.SetupDeleteUserByIdAsync(appUserDto.Id, expectedResult);

            // Act
            var result = await _usersController.DeleteUserByIdAsync(appUserDto.Id);

            // Assert
            result.Should().BeAssignableTo<OkObjectResult>();
            _userService.Verify(repo => repo.DeleteUserByIdAsync(appUserDto.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdUserAsync_FailedToDelete_ReturnBadRequest()
        {
            // Arrange
            var appUserDto = _appUserDto.Generate();
            var expectedResult = new InvalidResult<AppUserDto>(string.Empty);
            _userServiceHelper.SetupDeleteUserByIdAsync(appUserDto.Id, expectedResult);

            // Act
            var result = await _usersController.DeleteUserByIdAsync(appUserDto.Id);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _userService.Verify(repo => repo.DeleteUserByIdAsync(appUserDto.Id), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_UserExists_ReturnsOk()
        {
            // Arrange
            var user = _appUserDto.Generate();
            var expectedResult = new SuccessResult<AppUserDto>(user);
            _userServiceHelper.SetupDeleteUserByIdAsync(user.Email, expectedResult);

            // Act
            var result = await _usersController.GetUserByEmailAsync(user.Email);

            // Assert
            result
                .Should()
                .BeAssignableTo<OkObjectResult>()
                .Which.Value.Should()
                .BeEquivalentTo(user);
            _userService.Verify(repo => repo.GetUserByEmailAsync(user.Email), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_UserDoesNotExist_ReturnsOk()
        {
            // Arrange
            var user = _appUserDto.Generate();
            var expectedResult = new InvalidResult<AppUserDto>(string.Empty);
            _userServiceHelper.SetupDeleteUserByIdAsync(user.Email, expectedResult);

            // Act
            var result = await _usersController.GetUserByEmailAsync(user.Email);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _userService.Verify(repo => repo.GetUserByEmailAsync(user.Email), Times.Once);
        }
    }
}
