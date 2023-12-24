using AutoMapper;
using Bogus;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Tests.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Exceptions;
using Shared.SharedModels;

namespace Identity.Tests.ServiceTests;

public class EmailConfirmationServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly UserManagerHelper _userManagerHelper;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<ILogger<EmailConfirmationService>> _loggerMock;
    private readonly EmailConfirmationService _emailConfirmationService;
    private readonly Faker _faker;

    public EmailConfirmationServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        var storeMock = new Mock<IUserStore<AppUser>>();
        var optionsAccessorMock = new Mock<IOptions<IdentityOptions>>();
        var passwordHasherMock = new Mock<IPasswordHasher<AppUser>>();
        var userValidatorMocks = new List<Mock<IUserValidator<AppUser>>>();
        var passwordValidatorMocks = new List<Mock<IPasswordValidator<AppUser>>>();
        var keyNormalizerMock = new Mock<ILookupNormalizer>();
        var errorsMock = new Mock<IdentityErrorDescriber>();
        var servicesMock = new Mock<IServiceProvider>();
        var loggerMock = new Mock<ILogger<UserManager<AppUser>>>();

        _userManagerMock = new Mock<UserManager<AppUser>>(
            storeMock.Object,
            optionsAccessorMock.Object,
            passwordHasherMock.Object,
            userValidatorMocks.Select(mock => mock.Object),
            passwordValidatorMocks.Select(mock => mock.Object),
            keyNormalizerMock.Object,
            errorsMock.Object,
            servicesMock.Object,
            loggerMock.Object
        );

        _userManagerHelper = new UserManagerHelper(_userManagerMock);
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

    [Theory]
    [InlineData("", "test@example.com")]
    [InlineData("validToken", "")]
    [InlineData("validToken", null)]
    public async Task ConfirmEmailAsync_InvalidInput_ShouldThrowWrongActionException(
        string token,
        string email
    )
    {
        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<WrongActionException>(Act);
    }

    [Theory]
    [InlineData("valid_token", "test@example.com")]
    public async Task ConfirmEmailAsync_UserNotFound_ShouldThrowNotFoundException(
        string token,
        string email
    )
    {
        // Arrange
        AppUser expectedResult = null;
        _userManagerHelper.SetupFindByEmailAsync(email, expectedResult);

        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Theory]
    [InlineData("valid_token", "test@example.com")]
    public async Task ConfirmEmailAsync_InvalidToken_ShouldThrowWrongActionException(
        string token,
        string email
    )
    {
        // Arrange
        var user = new AppUser { Email = email };

        _userManagerHelper.SetupFindByEmailAsync(email, user);
        _userManagerHelper.SetupConfirmEmailAsync(user, token, IdentityResult.Failed());

        // Act
        async Task Act() => await _emailConfirmationService.ConfirmEmailAsync(token, email);

        // Assert
        await Assert.ThrowsAsync<WrongActionException>(Act);
    }

    [Theory]
    [InlineData("valid_token", "test@example.com")]
    public async Task ConfirmEmailAsync_ValidTokenAndEmail_ShouldConfirmEmail(
        string token,
        string email
    )
    {
        // Arrange
        var user = new AppUser { Email = email };

        _userManagerHelper.SetupFindByEmailAsync(email, user);
        _userManagerHelper.SetupConfirmEmailAsync(user, token, IdentityResult.Success);

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
