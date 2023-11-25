using Identity.Application.Ports.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailConfirmationService _emailService;

    public EmailController(IEmailConfirmationService emailService)
    {
        _emailService = emailService;
    }

    [HttpGet("email-confirmation")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string token,
        [FromQuery] string email
    )
    {
        await _emailService.ConfirmEmailAsync(token, email);

        return Ok(Constants.SuccessEmailConfirmation);
    }
}
