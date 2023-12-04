using AutoMapper;
using Bogus;
using FluentAssertions;
using Hangfire;
using Identity.Application.Dtos;
using Identity.Application.Ports.Utils;
using Identity.Application.ResultPattern.Results;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Tests.Helpers;
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
    private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock;
    private readonly UserService _userService;
    private readonly MapperHelper _mapperHelper;
    private readonly AppUserRepositoryHelper _appUserRepositoryHelper;
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
        _backgroundJobClientMock = new Mock<IBackgroundJobClient>();

        _userService = new UserService(
            _mapperMock.Object,
            _appUserRepositoryMock.Object,
            _publishEndpointMock.Object,
            _emailConfirmationHelperMock.Object,
            _loggerMock.Object,
            _httpContextAccessorMock.Object,
            _backgroundJobClientMock.Object
        );

        _mapperHelper = new MapperHelper(_mapperMock);
        _appUserRepositoryHelper = new AppUserRepositoryHelper(_appUserRepositoryMock);

        _appUserFaker = new Faker<AppUser>()
            .RuleFor(u => u.Id, f => f.Random.Number(max: int.MaxValue))
            .RuleFor(u => u.Email, f => f.Internet.Email());
    }

    [Fact]
    public async Task AddUserAsync_UserDoesNotExist_ReturnSuccessResult()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();
        var appUserDto = new AppUserRegisterDto { Email = appUser.Email };
        _mapperHelper.SetupMap(appUserDto, appUser);
        _appUserRepositoryHelper.SetupCreateUserAsync(IdentityResult.Success);

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
        var identityResult = IdentityResult.Failed();
        _mapperHelper.SetupMap(appUserDto, appUser);
        _appUserRepositoryHelper.SetupCreateUserAsync(identityResult);

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
        var appUsers = _appUserFaker.Generate(5);
        var appUserDtos = appUsers
            .Select(user => new AppUserDto { Id = user.Id, Email = user.Email })
            .ToList();
        _mapperHelper.SetupMap(appUsers, appUserDtos);
        _appUserRepositoryHelper.SetupGetAllUsersAsync(offset, limit, appUsers);

        // Act
        var result = await _userService.GetAllUsersAsync(offset, limit);

        // Assert
        result.Should().BeOfType<SuccessResult<List<AppUserDto>>>();
        result.Value.Should().Equal(appUserDtos);
        _appUserRepositoryMock.Verify(repo => repo.GetAllUsersAsync(offset, limit), Times.Once);
    }

    [Fact]
    public async Task GetAllUsersAsync_EmplyReposiory_ReturnEmplyList()
    {
        // Arrange
        var offset = 0;
        var limit = 10;
        List<AppUser> appUsers = new();
        _appUserRepositoryHelper.SetupGetAllUsersAsync(offset, limit, appUsers);

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
        _appUserRepositoryHelper.SetupGetUserByIdAsync(appUser.Id.ToString(), appUser);
        _mapperHelper.SetupMap(appUser, appUserDto);

        // Act
        var result = await _userService.GetUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().Be(appUserDto);
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
        AppUser appUser = null;
        _appUserRepositoryHelper.SetupGetUserByIdAsync(targetUserId.ToString(), appUser);

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
        _mapperHelper.SetupMap(appUser, appUserDto);
        _appUserRepositoryHelper.SetupGetUserByIdAsync(appUser.Id.ToString(), appUser);
        _appUserRepositoryHelper.SetupDeleteUserAsync(appUser, IdentityResult.Success);

        // Act
        var result = await _userService.DeleteUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().Be(appUserDto);
        _appUserRepositoryMock.Verify(
            repo => repo.GetUserByIdAsync(appUser.Id.ToString()),
            Times.Once
        );
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
        _appUserRepositoryHelper.SetupGetUserByIdAsync(targetUserId.ToString(), appUser);

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
    public async Task DeleteUserByIdAsync_AttempToDeleteAdmin_ImpossibleToDeleteAdmin()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();
        _appUserRepositoryHelper.SetupGetUserByIdAsync(appUser.Id.ToString(), appUser);
        _appUserRepositoryHelper.SetupIsUserInRoleAsync(appUser, Roles.AdminRole, true);

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
        _appUserRepositoryHelper.SetupGetUserByIdAsync(appUser.Id.ToString(), appUser);
        _appUserRepositoryHelper.SetupIsUserInRoleAsync(appUser, Roles.AdminRole, false);
        _appUserRepositoryHelper.SetupDeleteUserAsync(appUser, IdentityResult.Failed());

        // Act
        var result = await _userService.DeleteUserByIdAsync(appUser.Id);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
        _appUserRepositoryMock.Verify(repo => repo.DeleteUserAsync(appUser), Times.Once);
    }

    [Fact]
    public async Task GetUserByEmailAsync_UserExists_ReturnUser()
    {
        // Arrange
        var appUser = _appUserFaker.Generate();
        var appUserDto = new AppUserDto { Id = appUser.Id, Email = appUser.Email };
        _appUserRepositoryHelper.SetupGetUserByEmailAsync(appUser.Email, appUser);
        _mapperHelper.SetupMap(appUser, appUserDto);

        // Act
        var result = await _userService.GetUserByEmailAsync(appUser.Email);

        // Assert
        result.Should().BeOfType<SuccessResult<AppUserDto>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().Be(appUserDto);
        _appUserRepositoryMock.Verify(repo => repo.GetUserByEmailAsync(appUser.Email), Times.Once);
    }

    [Fact]
    public async Task GetUserByEmailAsync_UserDoesNotExist_ReturnInvalidResult()
    {
        // Arrange
        var targetUserEmail = _appUserFaker.Generate().Email;
        AppUser appUser = null;
        _appUserRepositoryHelper.SetupGetUserByEmailAsync(targetUserEmail, appUser);

        // Act
        var result = await _userService.GetUserByEmailAsync(targetUserEmail);

        // Assert
        result.Should().BeOfType<InvalidResult<AppUserDto>>();
        _appUserRepositoryMock.Verify(
            repo => repo.GetUserByEmailAsync(It.IsAny<string>()),
            Times.Once
        );
    }
}
