using Google.Protobuf.WellKnownTypes;
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

    public async Task<IdentityResult> CreateUserAsync(AppUser appUser, string password)
    {
        return await _userManager.CreateAsync(appUser, password);
    }

    public async Task<IdentityResult> DeleteUserAsync(AppUser appUser)
    {
        return await _userManager.DeleteAsync(appUser);
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
}
