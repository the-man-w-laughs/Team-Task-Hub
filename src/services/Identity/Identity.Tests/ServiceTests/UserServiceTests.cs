using AutoMapper;
using Bogus;
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
using Shared.SharedModels;

namespace Identity.Tests.ServiceTests;

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
    private readonly Faker<AppUser> _appUserFaker;

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

        _appUserFaker = new Faker<AppUser>()
            .RuleFor(u => u.Id, f => f.Random.Number())
            .RuleFor(u => u.Email, f => f.Internet.Email());
    }

    [Fact]
    public async Task AddUserAsync_UserDoesNotExist_ReturnSuccessResult()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();
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
        var appUser = _appUserFaker.Generate();
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
        int offset = 0;
        int limit = 10;
        var users = _appUserFaker.Generate(5);
        var userDtos = users
            .Select(user => new AppUserDto { Id = user.Id, Email = user.Email })
            .ToList();

        _appUserRepositoryMock
            .Setup(repo => repo.GetAllUsersAsync(offset, limit))
            .ReturnsAsync(users);
        _mapperMock.Setup(mapper => mapper.Map<List<AppUserDto>>(users)).Returns(userDtos);

        // Act
        var result = await _userService.GetAllUsersAsync(offset, limit);

        // Assert
        result.Should().BeOfType<SuccessResult<List<AppUserDto>>>();
        result.Value.Should().Equal(userDtos);
        _appUserRepositoryMock.Verify(repo => repo.GetAllUsersAsync(offset, limit), Times.Once);
    }

    [Fact]
    public async Task GetAllUsersAsync_EmplyReposiory_ReturnEmplyList()
    {
        // Arrange
        int offset = 0;
        int limit = 10;
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
        _appUserRepositoryMock.Verify(repo => repo.GetAllUsersAsync(offset, limit), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserExists_ReturnUser()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();
        var appUserDto = new AppUserDto { Id = appUser.Id, Email = appUser.Email };

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(appUser.Id.ToString()))
            .ReturnsAsync(appUser);
        _mapperMock.Setup(mapper => mapper.Map<AppUserDto>(appUser)).Returns(appUserDto);

        // Act
        var result = await _userService.GetUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(appUser.Id);
        _appUserRepositoryMock.Verify(
            repo => repo.GetUserByIdAsync(appUser.Id.ToString()),
            Times.Once
        );
    }

    [Fact]
    public async Task GetUserByIdAsync_UserDoesNotExist_ReturnInvalidResult()
    {
        // Arrange
        var targetUserId = _appUserFaker.Generate().Id;
        AppUser user = null;

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
        _appUserRepositoryMock.Verify(
            repo => repo.GetUserByIdAsync(It.IsAny<string>()),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteUserByIdAsync_TargetUserExists_ReturnSuccessResult()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();
        var appUserDto = new AppUserDto { Id = appUser.Id, Email = appUser.Email };

        _mapperMock.Setup(mapper => mapper.Map<AppUserDto>(appUser)).Returns(appUserDto);
        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(appUser.Id.ToString()))
            .ReturnsAsync(appUser);
        _appUserRepositoryMock
            .Setup(repo => repo.DeleteUserAsync(appUser))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.DeleteUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().Be(appUserDto);
        _appUserRepositoryMock.Verify(repo => repo.DeleteUserAsync(appUser), Times.Once);
        _publishEndpointMock.Verify(
            p => p.Publish(It.IsAny<UserDeletedMessage>(), default),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteUserByIdAsync_TargetUserDoesNotExist_ReturnInvalidResult()
    {
        // Arrange
        var targetUserId = _appUserFaker.Generate().Id;
        AppUser appUser = null;

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(targetUserId.ToString()))
            .ReturnsAsync(appUser);

        // Act
        var result = await _userService.DeleteUserByIdAsync(targetUserId);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
        _appUserRepositoryMock.Verify(
            repo => repo.DeleteUserAsync(It.IsAny<AppUser>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ImpossibleToDeleteAdmin()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(appUser.Id.ToString()))
            .ReturnsAsync(appUser);
        _appUserRepositoryMock
            .Setup(repo => repo.IsUserInRoleAsync(appUser, Roles.AdminRole.Name!))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
        _appUserRepositoryMock.Verify(
            repo => repo.DeleteUserAsync(It.IsAny<AppUser>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeleteUserByIdAsync_FailedToDeleteUser_ReturnInvalidResult()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();

        _appUserRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(appUser.Id.ToString()))
            .ReturnsAsync(appUser);
        _appUserRepositoryMock
            .Setup(repo => repo.IsUserInRoleAsync(appUser, Roles.AdminRole.Name!))
            .ReturnsAsync(false);

        var identityResult = IdentityResult.Failed();
        _appUserRepositoryMock
            .Setup(repo => repo.DeleteUserAsync(appUser))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _userService.DeleteUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
        _appUserRepositoryMock.Verify(repo => repo.DeleteUserAsync(appUser), Times.Once);
    }
}
