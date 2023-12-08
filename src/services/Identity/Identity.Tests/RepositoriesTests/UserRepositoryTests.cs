using Bogus;
using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Tests.Fakers;
using Identity.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.Tests.RepositoriesTests
{
    public class UserRepositoryTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly UserManagerHelper _userManagerHelper;
        private readonly Faker<AppUser> _appUserFaker;
        private readonly Faker _faker;
        private readonly AppUserRepository _appUserRepository;

        public UserRepositoryTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );

            _userManagerHelper = new UserManagerHelper(_userManagerMock);
            _appUserRepository = new AppUserRepository(_userManagerMock.Object);
            _appUserFaker = new AppUserFaker();
            _faker = new Faker();
        }

        [Fact]
        public async void GetAllAsync_RepositoryHasValues_ShouldReturnUsers()
        {
            // Arrange
            var offset = 1;
            var limit = 2;
            var initialCount = 5;
            var users = _appUserFaker
                .Generate(initialCount)
                .Select(user =>
                {
                    user.EmailConfirmed = true;
                    return user;
                })
                .ToList();
            var expectedResult = users
                .Where(user => user.EmailConfirmed == true)
                .Skip(offset)
                .Take(limit)
                .ToList();
            _userManagerHelper.SetupGetAllAsync(users);

            // Act
            var result = await _appUserRepository.GetAllUsersAsync(offset, limit);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _userManagerMock.Verify(repo => repo.Users, Times.Once);
        }

        [Fact]
        public async void GetAllAsync_EmptyRepository_ShouldReturnUsers()
        {
            // Arrange
            var offset = 1;
            var limit = 2;
            var users = new List<AppUser>();
            var expectedResult = users;
            _userManagerHelper.SetupGetAllAsync(users);

            // Act
            var result = await _appUserRepository.GetAllUsersAsync(offset, limit);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _userManagerMock.Verify(repo => repo.Users, Times.Once);
        }

        [Fact]
        public async void CreateUserAsync_UserDoesNotExist_ShouldReturnSuccess()
        {
            // Arrange
            var initialCount = 5;
            var users = _appUserFaker.Generate(initialCount);
            var usersToPass = users.ToList();
            var user = _appUserFaker.Generate();
            var password = _faker.Internet.Password();
            _userManagerHelper.SetupCreateAsync(usersToPass);

            // Act
            var result = await _appUserRepository.CreateUserAsync(user, password);

            // Assert
            result.Should().Be(IdentityResult.Success);
            _userManagerMock.Verify(repo => repo.CreateAsync(user, password), Times.Once);
            usersToPass.Should().BeEquivalentTo(users.Concat(new[] { user }));
        }

        [Fact]
        public async void DeleteUserAsync_UserExists_ShouldReturnSuccess()
        {
            // Arrange
            var initialCount = 5;
            var users = _appUserFaker.Generate(initialCount);
            var usersToPass = users.ToList();
            var user = users.FirstOrDefault();
            _userManagerHelper.SetupDeleteAsync(usersToPass);

            // Act
            var result = await _appUserRepository.DeleteUserAsync(user);

            // Assert
            result.Should().Be(IdentityResult.Success);
            _userManagerMock.Verify(repo => repo.DeleteAsync(user), Times.Once);
            usersToPass.Should().BeEquivalentTo(users.Where(u => u != user).ToList());
        }

        [Fact]
        public async void FindByEmailAsync_UserExists_ShouldReturnUser()
        {
            // Arrange
            var initialCount = 5;
            var users = _appUserFaker.Generate(initialCount);
            var user = users.FirstOrDefault();
            _userManagerHelper.SetupFindByEmailAsync(users);

            // Act
            var result = await _appUserRepository.GetUserByEmailAsync(user.Email);

            // Assert
            result.Should().Be(user);
            _userManagerMock.Verify(repo => repo.FindByEmailAsync(user.Email), Times.Once);
            users.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async void FindByEmailAsync_UserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var initialCount = 5;
            var users = _appUserFaker.Generate(initialCount);
            var user = _appUserFaker.Generate();
            _userManagerHelper.SetupFindByEmailAsync(users);

            // Act
            var result = await _appUserRepository.GetUserByEmailAsync(user.Email);

            // Assert
            result.Should().Be(null);
            _userManagerMock.Verify(repo => repo.FindByEmailAsync(user.Email), Times.Once);
            users.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async void FindByIdAsync_UserExists_ShouldReturnUser()
        {
            // Arrange
            var initialCount = 5;
            var users = _appUserFaker.Generate(initialCount);
            var user = users.FirstOrDefault();
            _userManagerHelper.SetupFindByIdAsync(users);

            // Act
            var result = await _appUserRepository.GetUserByIdAsync(user.Id.ToString());

            // Assert
            result.Should().Be(user);
            _userManagerMock.Verify(repo => repo.FindByIdAsync(user.Id.ToString()), Times.Once);
            users.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async void FindByIdAsync_UserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var initialCount = 5;
            var users = _appUserFaker.Generate(initialCount);
            var user = _appUserFaker.Generate();
            _userManagerHelper.SetupFindByIdAsync(users);

            // Act
            var result = await _appUserRepository.GetUserByIdAsync(user.Id.ToString());

            // Assert
            result.Should().Be(null);
            _userManagerMock.Verify(repo => repo.FindByIdAsync(user.Id.ToString()), Times.Once);
            users.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async void IsInRoleAsync_UserHasTargetRole_ReturnTrue()
        {
            // Arrange
            var targetRole = "admin";
            var user = _appUserFaker.Generate();
            var roles = new Dictionary<AppUser, string>() { { user, targetRole } };
            _userManagerHelper.SetupIsInRoleAsync(roles);

            // Act
            var result = await _appUserRepository.IsUserInRoleAsync(user, targetRole);

            // Assert
            result.Should().Be(true);
            _userManagerMock.Verify(repo => repo.IsInRoleAsync(user, targetRole), Times.Once);
        }

        [Fact]
        public async void IsInRoleAsync_UserDoesntHaveTargetRole_ReturnFalse()
        {
            // Arrange
            var currentRole = "user";
            var targetRole = "admin";
            var user = _appUserFaker.Generate();
            var roles = new Dictionary<AppUser, string>() { { user, currentRole } };
            _userManagerHelper.SetupIsInRoleAsync(roles);

            // Act
            var result = await _appUserRepository.IsUserInRoleAsync(user, targetRole);

            // Assert
            result.Should().Be(false);
            _userManagerMock.Verify(repo => repo.IsInRoleAsync(user, targetRole), Times.Once);
        }
    }
}
