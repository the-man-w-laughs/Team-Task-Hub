using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.MediatR.CQRS.Projects.Commands;
using TeamHub.BLL.MediatR.CQRS.Projects.Queries;

namespace TeamHub.WebApi.controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TaskModelController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskModelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create Tasks Comment
    /// </summary>
    [HttpPost("{taskId:int}/comments")]
    public async Task<IActionResult> CreateNewTasksComment([FromRoute] int taskId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Get All Tasks Comments
    /// </summary>
    [HttpGet("{taskId:int}/comments")]
    public async Task<IActionResult> GetAllTasksComments([FromRoute] int taskId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Get Task
    /// </summary>
    [HttpGet("{taskId:int}")]
    public async Task<IActionResult> GetTask([FromRoute] int taskId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Update Task
    /// </summary>
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] int taskId,
        [FromBody] TaskModelRequestDto taskModelRequestDto
    )
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Delete Task
    /// </summary>
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTask([FromRoute] int taskId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Create Task Handler
    /// </summary>
    [HttpPost("{taskId:int}/TaskHandlers")]
    public async Task<IActionResult> CreateTaskHandler(
        [FromRoute] int taskId,
        [FromBody] TasksHandlerRequestDto tasksHandlerRequestDto
    )
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Get All Task Handlers
    /// </summary>
    [HttpGet("{taskId:int}/TaskHandlers")]
    public async Task<IActionResult> GetAllTaskHandlers([FromRoute] int taskId)
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }

    /// <summary>
    /// Delete Task Handler
    /// </summary>
    [HttpDelete("{taskId:int}/TaskHandlers/{userId:int}")]
    public async Task<IActionResult> DeleteTaskHandler(
        [FromRoute] int taskId,
        [FromRoute] int userId
    )
    {
        // var command = new CreateProjectCommand(projectRequestDto);
        // var result = await _mediator.Send(command);
        // return Ok(result);
        return Ok();
    }
}
