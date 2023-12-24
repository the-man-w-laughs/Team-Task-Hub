using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.Tests.Helpers
{
    public class AppUserRepositoryHelper
    {
        private readonly Mock<IAppUserRepository> _appUserRepositoryMock;

        public AppUserRepositoryHelper(Mock<IAppUserRepository> appUserRepositoryMock)
        {
            _appUserRepositoryMock = appUserRepositoryMock;
        }

        public void SetupCreateUserAsync(IdentityResult expectedResult)
        {
            _appUserRepositoryMock
                .Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult);
        }

        public void SetupGetAllUsersAsync(int offset, int limit, List<AppUser> expectedResult)
        {
            _appUserRepositoryMock
                .Setup(repo => repo.GetAllUsersAsync(offset, limit))
                .ReturnsAsync(expectedResult);
        }

        public void SetupGetUserByIdAsync(string id, AppUser expectedResult)
        {
            _appUserRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(id))
                .ReturnsAsync(expectedResult);
        }

        public void SetupDeleteUserAsync(AppUser appUserToDelete, IdentityResult expectedResult)
        {
            _appUserRepositoryMock
                .Setup(repo => repo.DeleteUserAsync(appUserToDelete))
                .ReturnsAsync(expectedResult);
        }

        public void SetupIsUserInRoleAsync(
            AppUser appUser,
            IdentityRole<int> identityRole,
            bool expectedResult
        )
        {
            _appUserRepositoryMock
                .Setup(repo => repo.IsUserInRoleAsync(appUser, identityRole.Name))
                .ReturnsAsync(expectedResult);
        }

        public void SetupGetUserByEmailAsync(string email, AppUser expectedResult)
        {
            _appUserRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync(expectedResult);
        }
    }
}
