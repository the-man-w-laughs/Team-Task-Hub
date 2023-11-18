using Identity.Application.Ports.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string token,
        [FromQuery] string email
    )
    {
        await _emailService.ConfirmEmail(token, email);

        return Ok(
            "Congratulations! Your email has been successfully confirmed. Thank you for verifying your email address!"
        );
    }
}
