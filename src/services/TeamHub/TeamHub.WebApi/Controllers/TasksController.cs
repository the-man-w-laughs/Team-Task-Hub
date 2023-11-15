using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.MediatR.CQRS.Comments.Commands;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;
using TeamHub.BLL.MediatR.CQRS.TaskHandlers.Queries;
using TeamHub.BLL.MediatR.CQRS.Tasks.Commands;
using TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

namespace TeamHub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get Task
    /// </summary>
    [HttpGet("{taskId:int}")]
    public async Task<IActionResult> GetTaskAsync([FromRoute] int taskId)
    {
        var command = new GetTaskByIdQuery(taskId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Update Task
    /// </summary>
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateTaskAsync(
        [FromRoute] int taskId,
        [FromBody] TaskModelRequestDto taskModelRequestDto
    )
    {
        var command = new UpdateTaskCommand(taskId, taskModelRequestDto);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Delete Task
    /// </summary>
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] int taskId)
    {
        var command = new DeleteTaskCommand(taskId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Create Task Handler
    /// </summary>
    [HttpPost("{taskId:int}/handlers/{userId}")]
    public async Task<IActionResult> CreateTaskHandlerAsync(
        [FromRoute] int taskId,
        [FromRoute] int userId
    )
    {
        var command = new CreateTaskHandlerCommand(taskId, userId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get All Task Handlers
    /// </summary>
    [HttpGet("{taskId:int}/handlers")]
    public async Task<IActionResult> GetAllTaskHandlersAsync(
        [FromRoute] int taskId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var command = new GetAllTaskHandlersQuery(taskId, offset, limit);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Delete Task Handler
    /// </summary>
    [HttpDelete("{taskId:int}/handlers/{userId:int}")]
    public async Task<IActionResult> DeleteTaskHandlerAsync(
        [FromRoute] int taskId,
        [FromRoute] int userId
    )
    {
        var command = new DeleteTaskHandlerCommand(taskId, userId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Create Tasks Comment
    /// </summary>
    [HttpPost("{taskId:int}/comments")]
    public async Task<IActionResult> CreateNewTasksCommentAsync(
        [FromRoute] int taskId,
        [FromBody] CommentRequestDto commentRequestDto
    )
    {
        var command = new CreateCommentCommand(taskId, commentRequestDto);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get All Tasks Comments
    /// </summary>
    [HttpGet("{taskId:int}/comments")]
    public async Task<IActionResult> GetAllTasksCommentsAsync(
        [FromRoute] int taskId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var command = new GetAllTasksCommentsQuery(taskId, offset, limit);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }
}
