using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Namespace;

public class AppUserRepository : IAppUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AppUserRepository(UserManager<AppUser> userManager)
    {
        this._userManager = userManager;
    }

    public async Task CreateUserAsync(AppUser appUser, string password)
    {
        var identityResult = await _userManager.CreateAsync(appUser, password);
        HandleIdentityResult("Create user", identityResult);
    }

    public async Task DeleteUserAsync(AppUser appUser)
    {
        var identityResult = await _userManager.DeleteAsync(appUser);
        HandleIdentityResult("Delete user", identityResult);
    }

    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<bool> IsUserInRoleAsync(AppUser appUser, string role)
    {
        return await _userManager.IsInRoleAsync(appUser, role);
    }

    private void HandleIdentityResult(string action, IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new Exception(
                $"Failed to {action}: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}"
            );
        }
    }
}
