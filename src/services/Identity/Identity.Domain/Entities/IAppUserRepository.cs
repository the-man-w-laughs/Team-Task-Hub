using Identity.Domain.Entities;

namespace Namespace;

public interface IAppUserRepository
{
    Task CreateUserAsync(AppUser appUser, string password);
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(string id);
    Task DeleteUserAsync(AppUser appUser);
    Task<bool> IsUserInRoleAsync(AppUser appUser, string role);
}
