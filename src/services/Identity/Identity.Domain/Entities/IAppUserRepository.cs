using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Namespace;

public interface IAppUserRepository
{
    Task<IdentityResult> CreateUserAsync(AppUser appUser, string password);
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(string id);
    Task<IdentityResult> DeleteUserAsync(AppUser appUser);
    Task<bool> IsUserInRoleAsync(AppUser appUser, string role);
}
