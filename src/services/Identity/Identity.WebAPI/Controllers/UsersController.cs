using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.IdentityConstraints;

namespace Identity.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Create new user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateNewUserAsync(AppUserRegisterDto appUserRegisterDto)
    {
        var result = await _userService.AddUserAsync(appUserRegisterDto);

        return this.FromResult(result);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUsersAsync(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var result = await _userService.GetAllUsersAsync(offset, limit);

        return this.FromResult(result);
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);

        return this.FromResult(result);
    }

    /// <summary>
    /// Delete user by id
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.AdminOnly)]
    public async Task<IActionResult> DeleteUserByIdAsync(int id)
    {
        var result = await _userService.DeleteUserByIdAsync(id);

        return this.FromResult(result);
    }
}
