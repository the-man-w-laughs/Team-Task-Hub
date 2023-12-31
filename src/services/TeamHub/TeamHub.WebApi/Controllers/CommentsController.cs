using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.MediatR.CQRS.Comments.Commands;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;

namespace TeamHub.WebApi.Controllers;

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
    public async Task<IActionResult> GetCommentAsync([FromRoute] int commentId)
    {
        var command = new GetCommentByIdQuery(commentId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Update Comment
    /// </summary>
    [HttpPut("{commentId:int}")]
    public async Task<IActionResult> UpdateCommentAsync(
        [FromRoute] int commentId,
        [FromBody] CommentRequestDto commentRequestDto
    )
    {
        var command = new UpdateCommentCommand(commentId, commentRequestDto);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Delete Comment
    /// </summary>
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteCommentAsync([FromRoute] int commentId)
    {
        var command = new DeleteCommentCommand(commentId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }
}
