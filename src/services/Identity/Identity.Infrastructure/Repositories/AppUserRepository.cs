using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AppUserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser appUser, string password)
    {
        return await _userManager.CreateAsync(appUser, password);
    }

    public async Task<IdentityResult> DeleteUserAsync(AppUser appUser)
    {
        return await _userManager.DeleteAsync(appUser);
    }

    public async Task<IEnumerable<AppUser>> GetAllUsersAsync(int offset, int limit)
    {
        return await _userManager.Users
            .Where(user => user.EmailConfirmed == true)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
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
