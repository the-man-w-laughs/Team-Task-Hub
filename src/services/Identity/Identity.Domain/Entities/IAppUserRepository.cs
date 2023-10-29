namespace Identity.Domain.Entities;

public interface IAppUserRepository
{
    Task CreateUserAsync(AppUser appUser, string password);
    Task<IEnumerable<AppUser>> GetAllUsersAsync(int offset, int limit);
    Task<AppUser?> GetUserByIdAsync(string id);
    Task DeleteUserAsync(AppUser appUser);
    Task<bool> IsUserInRoleAsync(AppUser appUser, string role);
}
