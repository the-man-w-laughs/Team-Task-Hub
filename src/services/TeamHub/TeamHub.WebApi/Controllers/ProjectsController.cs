using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.MediatR.CQRS.Projects.Commands;
using TeamHub.BLL.MediatR.CQRS.Projects.Queries;
using TeamHub.BLL.MediatR.CQRS.Tasks.Commands;
using TeamHub.BLL.MediatR.CQRS.Tasks.Queries;
using TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;
using TeamHub.BLL.MediatR.CQRS.TeamMembers.Queries;

namespace TeamHub.WebApi.Controllers;

/// <summary>
/// Controller for managing projects.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor for ProjectsController.
    /// </summary>
    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new Project
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateNewProjectAsync(
        [FromBody] ProjectRequestDto projectRequestDto
    )
    {
        var command = new CreateProjectCommand(projectRequestDto);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get Project By Id
    /// </summary>
    [HttpGet("{projectId:int}")]
<<<<<<< HEAD:src/services/TeamHub/TeamHub.WebApi/Controllers/ProjectsController.cs
    public async Task<IActionResult> GetProjectByIdAsync(
        [FromRoute] int projectId,
        CancellationToken cancellationToken
    )
=======
    public async Task<IActionResult> GetProjectById([FromRoute] int projectId)
>>>>>>> feature/reports-microservice:src/services/TeamHub/TeamHub.WebApi/controllers/ProjectsController.cs
    {
        var command = new GetProjectByIdQuery(projectId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get Project By Id
    /// </summary>
    [HttpGet("{projectId:int}/full")]
    public async Task<IActionResult> GetFullProjectByIdAsync([FromRoute] int projectId)
    {
        var command = new GetFullProjectByIdQuery(projectId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get All Users Projects
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsersProjecsAsyncAsync(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var command = new GetAllUsersProjectsQuery(offset, limit);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Update Project
    /// </summary>
    [HttpPut("{projectId:int}")]
    public async Task<IActionResult> UpdateProjectAsync(
        [FromRoute] int projectId,
        [FromBody] ProjectRequestDto projectRequestDto
    )
    {
        var command = new UpdateProjectCommand(projectId, projectRequestDto);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Delete Project
    /// </summary>
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] int projectId)
    {
        var command = new DeleteProjectCommand(projectId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Create New Team Member
    /// </summary>
    [HttpPost("{projectId:int}/members/{userId:int}")]
    public async Task<IActionResult> CreateNewTeamMemberAsync(
        [FromRoute] int projectId,
        [FromRoute] int userId
    )
    {
        var command = new CreateTeamMemberCommand(projectId, userId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get All TeamMembers
    /// </summary>
    [HttpGet("{projectId:int}/members")]
    public async Task<IActionResult> GetAllTeamMembersAsync(
        [FromRoute] int projectId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var command = new GetAllProjectsTeamMembersQuery(projectId, offset, limit);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Delete TeamMember
    /// </summary>
    [HttpDelete("{projectId:int}/members/{userId:int}")]
    public async Task<IActionResult> DeleteTeamMemberAsync(
        [FromRoute] int projectId,
        [FromRoute] int userId
    )
    {
        var command = new DeleteTeamMemberCommand(projectId, userId);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Create New Task
    /// </summary>
    [HttpPost("{projectId:int}/tasks")]
    public async Task<IActionResult> CreateNewTaskModelAsync(
        [FromRoute] int projectId,
        [FromBody] TaskModelRequestDto taskModelRequestDto
    )
    {
        var command = new CreateTaskCommand(projectId, taskModelRequestDto);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// Get All Project tasks
    /// </summary>
    [HttpGet("{projectId:int}/tasks")]
    public async Task<IActionResult> GetAllProjectsTaskModelsAsync(
        [FromRoute] int projectId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var command = new GetAllProjectsTasksQuery(projectId, offset, limit);
        var result = await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok(result);
    }
}
