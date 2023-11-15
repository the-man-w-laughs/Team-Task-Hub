using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.BLL.MediatR.CQRS.Users.Queries;

namespace TeamHub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get Users
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetComment(CancellationToken cancellationToken)
    {
        var command = new GetAllUsersQuery();
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
