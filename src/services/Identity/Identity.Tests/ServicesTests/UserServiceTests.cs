using AutoMapper;
using FluentAssertions;
using Hangfire;
using Identity.Application.Dtos;
using Identity.Application.Ports.Utils;
using Identity.Application.ResultPattern.Results;
using Identity.Application.Services;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Helpers;
using Shared.IdentityConstraints;

namespace Identity.Tests.ServicesTests;

public class UserServiceTests
{
    private const int UserId = 1;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAppUserRepository> _appUserRepositoryMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<IConfirmationEmailSender> _emailConfirmationHelperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IBackgroundJobClient> _backgroundJobClient;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _appUserRepositoryMock = new Mock<IAppUserRepository>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _emailConfirmationHelperMock = new Mock<IConfirmationEmailSender>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(
            UserId
        );
        _backgroundJobClient = new Mock<IBackgroundJobClient>();

        _userService = new UserService(
            _mapperMock.Object,
            _appUserRepositoryMock.Object,
            _publishEndpointMock.Object,
            _emailConfirmationHelperMock.Object,
            _loggerMock.Object,
            _httpContextAccessorMock.Object,
            _backgroundJobClient.Object
        );
    }

    [Fact]
    public async Task AddUserAsync_UserDoesNotExist_ReturnSuccessResult()
    {
        // Arrange
        var appUser = new AppUser { Email = "test@example.com" };
        var appUserDto = new AppUserRegisterDto { Email = appUser.Email };

        _mapperMock.Setup(mapper => mapper.Map<AppUser>(appUserDto)).Returns(appUser);

        _appUserRepositoryMock
            .Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.AddUserAsync(appUserDto);

        // Assert
        result.Should().BeOfType<SuccessResult<string>>();
        _appUserRepositoryMock.Verify(
            repo => repo.CreateUserAsync(appUser, appUserDto.Password),
            Times.Once
        );
    }

    [Fact]
    public async Task AddUserAsync_UserExists_ReturnInvalidResult()
    {
        // Arrange
        var appUser = new AppUser { Email = "test@example.com" };
        var appUserDto = new AppUserRegisterDto { Email = appUser.Email };

        _mapperMock.Setup(mapper => mapper.Map<AppUser>(appUserDto)).Returns(appUser);

        var identityResult = IdentityResult.Failed();
        _appUserRepositoryMock
            .Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _userService.AddUserAsync(appUserDto);

        // Assert
        result.Should().BeOfType<InvalidResult<string>>();
        _appUserRepositoryMock.Verify(
            repo => repo.CreateUserAsync(It.IsAny<AppUser>(), It.IsAny<string>()),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllUsersAsync_NotEmplyReposiory_ReturnUsers()
    {
        // Arrange
        var offset = 0;
        var limit = 10;

        var users = new List<AppUser>
        {
            new() { Id = 1, Email = "user1@example.com" },
            new() { Id = 2, Email = "user2@example.com" }
        };

        var userDtos = new List<AppUserDto>
        {
            new() { Id = 1, Email = "user1@example.com" },
            new() { Id = 2, Email = "user2@example.com" }
        };

        _appUserRepositoryMock
            .Setup(repo => repo.GetAllUsersAsync(offset, limit))
            .ReturnsAsync(users);
        _mapperMock.Setup(mapper => mapper.Map<List<AppUserDto>>(users)).Returns(userDtos);

        // Act
        var result = await _userService.GetAllUsersAsync(offset, limit);

        // Assert
        result.Should().BeOfType<SuccessResult<List<AppUserDto>>>();
        result.Value.Should().Equal(userDtos);
    }

    [Fact]
    public async Task GetAllUsersAsync_EmplyReposiory_ReturnEmplyList()
    {
        // Arrange
        var offset = 0;
        var limit = 10;

        List<AppUser> users = new();

        _appUserRepositoryMock
            .Setup(repo => repo.GetAllUsersAsync(offset, limit))
            .ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsersAsync(offset, limit);

        // Assert
        result.Should().BeOfType<SuccessResult<List<AppUserDto>>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserByIdAsync_UserExists_ReturnUser()
    {
        // Arrange
        var targetUserId = 1;
        var appUser = new AppUser { Email = "test@example.com" };
        var appUserDto = new AppUserRegisterDto { Email = appUser.Email };

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(appUser);
        _mapperMock
            .Setup(mapper => mapper.Map<AppUserDto>(appUser))
            .Returns(new AppUserDto { Id = targetUserId, Email = "user@example.com" });

        // Act
        var result = await _userService.GetUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(targetUserId);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserDoesNotExist_ReturnInvalidResult()
    {
        // Arrange
        var targetUserId = 123;

        AppUser user = null;

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
    }

    [Fact]
    public async Task DeleteUserByIdAsync_TargetUserExists_ReturnSuccessResult()
    {
        // Arrange
        var targetUserId = 123;

        var appUser = new AppUser { Id = targetUserId, Email = "test@example.com" };
        var appUserDto = new AppUserDto { Id = targetUserId, Email = appUser.Email };

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(appUser);
        _appUserRepositoryMock
            .Setup(repo => repo.IsUserInRoleAsync(appUser, Roles.AdminRole.Name!))
            .ReturnsAsync(false);
        _appUserRepositoryMock
            .Setup(repo => repo.DeleteUserAsync(appUser))
            .ReturnsAsync(IdentityResult.Success);
        _mapperMock.Setup(mapper => mapper.Map<AppUserDto>(appUser)).Returns(appUserDto);

        // Act
        var result = await _userService.DeleteUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(targetUserId);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_TargetUserDoesNotExist_ReturnInvalidResult()
    {
        // Arrange
        var targetUserId = 123;

        AppUser user = null; // Simulate user not found

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.DeleteUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ImpossibleToDeleteAdmin()
    {
        // Arrange
        var targetUserId = 123;

        var user = new AppUser { Id = targetUserId, Email = "user@example.com" };

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(user);
        _appUserRepositoryMock
            .Setup(repo => repo.IsUserInRoleAsync(user, Roles.AdminRole.Name!))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
    }

    [Fact]
    public async Task DeleteUserByIdAsync_FailedToDeleteUser_ReturnInvalidResult()
    {
        // Arrange
        var targetUserId = 123;

        var user = new AppUser { Id = targetUserId, Email = "user@example.com" };

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(user);
        _appUserRepositoryMock
            .Setup(repo => repo.IsUserInRoleAsync(user, Roles.AdminRole.Name!))
            .ReturnsAsync(false);

        var identityResult = IdentityResult.Failed();
        _appUserRepositoryMock
            .Setup(repo => repo.DeleteUserAsync(user))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _userService.DeleteUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
    }
}
