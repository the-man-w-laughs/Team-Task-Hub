using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;

namespace Identity.Tests.Helpers
{
    public class UserManagerHelper
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;

        public UserManagerHelper(Mock<UserManager<AppUser>> userManagerMock)
        {
            _userManagerMock = userManagerMock;
        }

        public void SetupGetAllAsync(List<AppUser> users)
        {
            _userManagerMock.Setup(u => u.Users).Returns(users.AsQueryable().BuildMock());
        }

        public void SetupCreateAsync(List<AppUser> users)
        {
            _userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(
                    (AppUser user, string password) =>
                    {
                        users.Add(user);
                        return IdentityResult.Success;
                    }
                );
        }

        public void SetupDeleteAsync(List<AppUser> users)
        {
            _userManagerMock
                .Setup(u => u.DeleteAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(
                    (AppUser user) =>
                    {
                        users.Remove(user);
                        return IdentityResult.Success;
                    }
                );
        }

        public void SetupFindByEmailAsync(List<AppUser> users)
        {
            _userManagerMock
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    (string email) =>
                    {
                        return users.FirstOrDefault(u => u.Email == email);
                    }
                );
        }

        public void SetupFindByIdAsync(List<AppUser> users)
        {
            _userManagerMock
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    (string id) =>
                    {
                        return users.FirstOrDefault(u => u.Id.ToString() == id);
                    }
                );
        }

        public void SetupIsInRoleAsync(Dictionary<AppUser, string> roles)
        {
            _userManagerMock
                .Setup(u => u.IsInRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(
                    (AppUser user, string role) =>
                    {
                        return roles[user] == role;
                    }
                );
        }

        public void SetupFindByEmailAsync(string email, AppUser expectedResult)
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(expectedResult);
        }

        public void SetupConfirmEmailAsync(
            AppUser user,
            string token,
            IdentityResult expectedResult
        )
        {
            _userManagerMock
                .Setup(m => m.ConfirmEmailAsync(user, token))
                .ReturnsAsync(expectedResult);
        }
    }
}
