using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeamHub.WebApi.controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get Comment
    /// </summary>
    [HttpGet("{commentId:int}")]
    public async Task<IActionResult> GetComment([FromRoute] int commentId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Update Comment
    /// </summary>
    [HttpPut("{commentId:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int commentId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Delete Comment
    /// </summary>
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }
}
