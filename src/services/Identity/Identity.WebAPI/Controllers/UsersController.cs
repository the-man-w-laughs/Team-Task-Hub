using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Domain.Constraints;
using Identity.WebAPI.Extensions;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        this._userService = userService;
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
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var result = await _userService.GetAllUsersAsync();
        return this.FromResult(result);
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    [HttpGet("{id}")] 
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
