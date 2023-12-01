using AutoMapper;
using Bogus;
using Identity.Application.Services;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Shared.SharedModels;

namespace Identity.Tests.ServiceTests;

public class EmailConfirmationServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<ILogger<EmailConfirmationService>> _loggerMock;
    private readonly EmailConfirmationService _emailConfirmationService;
    private readonly Faker _faker;

    public EmailConfirmationServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _loggerMock = new Mock<ILogger<EmailConfirmationService>>();

        _emailConfirmationService = new EmailConfirmationService(
            _mapperMock.Object,
            _userManagerMock.Object,
            _publishEndpointMock.Object,
            _loggerMock.Object
        );

        _faker = new Faker();
    }

    [Fact]
    public async Task ConfirmEmailAsync_MissingEmail_ShouldThrowWrongActionException()
    {
        // Arrange
        var email = "";
        var token = "valid_token";
        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<WrongActionException>(Act);
    }

    [Fact]
    public async Task ConfirmEmailAsync_MissingToken_ShouldThrowWrongActionException()
    {
        // Arrange
        var email = _faker.Internet.Email();
        var token = "";
        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<WrongActionException>(Act);
    }

    [Fact]
    public async Task ConfirmEmailAsync_UserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var email = _faker.Internet.Email();
        var token = "valid_token";

        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync((AppUser)null);

        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Fact]
    public async Task ConfirmEmailAsync_InvalidToken_ShouldThrowWrongActionException()
    {
        // Arrange
        var email = _faker.Internet.Email();
        var token = "invalid_token";
        var user = new AppUser { Email = email };

        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.ConfirmEmailAsync(user, token))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<WrongActionException>(Act);
    }

    [Fact]
    public async Task ConfirmEmailAsync_ValidTokenAndEmail_ShouldConfirmEmail()
    {
        // Arrange
        var email = _faker.Internet.Email();
        var token = "valid_token";
        var user = new AppUser { Email = email };

        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.ConfirmEmailAsync(user, token))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        _userManagerMock.Verify(m => m.FindByEmailAsync(email), Times.Once);
        _userManagerMock.Verify(m => m.ConfirmEmailAsync(user, token), Times.Once);
        _publishEndpointMock.Verify(
            p => p.Publish(It.IsAny<UserCreatedMessage>(), default),
            Times.Once
        );
    }
}
