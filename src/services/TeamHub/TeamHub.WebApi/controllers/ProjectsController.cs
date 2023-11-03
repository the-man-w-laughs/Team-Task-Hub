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

namespace TeamHub.WebApi.controllers;

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
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Get Project By Id
    /// </summary>
    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> GetProjectById([FromRoute] int projectId)
    {
        var command = new GetProjectByIdQuery(projectId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Get Project By Id
    /// </summary>
    [HttpGet("{projectId:int}/full")]
    public async Task<IActionResult> GetFullProjectById([FromRoute] int projectId)
    {
        var command = new GetFullProjectByIdQuery(projectId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Get All Users Projects
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsersProjecsAsync()
    {
        var command = new GetAllUsersProjectsQuery();
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Update Project
    /// </summary>
    [HttpPut("{projectId:int}")]
    public async Task<IActionResult> UpdateProject(
        [FromRoute] int projectId,
        [FromBody] ProjectRequestDto projectRequestDto
    )
    {
        var command = new UpdateProjectCommand(projectId, projectRequestDto);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Delete Project
    /// </summary>
    [HttpDelete("{projectId:int}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int projectId)
    {
        var command = new DeleteProjectCommand(projectId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Create New Team Member
    /// </summary>
    [HttpPost("{projectId:int}/TeamMembers/{userId:int}")]
    public async Task<IActionResult> CreateNewTeamMember(
        [FromRoute] int projectId,
        [FromRoute] int userId
    )
    {
        var command = new CreateTeamMemberCommand(projectId, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Get All TeamMembers
    /// </summary>
    [HttpGet("{projectId:int}/TeamMembers")]
    public async Task<IActionResult> GetAllTeamMembers([FromRoute] int projectId)
    {
        var command = new GetAllProjectsTeamMembersQuery(projectId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Delete TeamMember
    /// </summary>
    [HttpDelete("{projectId:int}/TeamMembers/{userId:int}")]
    public async Task<IActionResult> DeleteTeamMember(
        [FromRoute] int projectId,
        [FromRoute] int userId
    )
    {
        var command = new DeleteTeamMemberCommand(projectId, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Create New Task
    /// </summary>
    [HttpPost("{projectId:int}/Tasks")]
    public async Task<IActionResult> CreateNewTaskModel(
        [FromRoute] int projectId,
        [FromBody] TaskModelRequestDto taskModelRequestDto
    )
    {
        var command = new CreateTaskCommand(projectId, taskModelRequestDto);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Get All Project tasks
    /// </summary>
    [HttpGet("{projectId:int}/Tasks")]
    public async Task<IActionResult> GetAllProjectsTaskModels([FromRoute] int projectId)
    {
        var command = new GetAllProjectsTasksQuery(projectId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
