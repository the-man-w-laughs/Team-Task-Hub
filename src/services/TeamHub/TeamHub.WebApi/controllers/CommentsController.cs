using Microsoft.AspNetCore.Mvc;

namespace TeamHub.WebApi.controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    /// <summary>
    /// Create new user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateNewUserAsync(AppUserRegisterDto appUserRegisterDto)
    {
        var result = await _userService.AddUserAsync(appUserRegisterDto);
        return this.FromResult(result);
    }
}
