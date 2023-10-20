using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class CreateTaskHandlerCommandHandler : IRequestHandler<CreateTaskHandlerCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITaskHandlerRepository _taskHandlerRepository;
    private readonly ITaskModelRepository _taskRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public CreateTaskHandlerCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ITaskHandlerRepository taskHandlerRepository,
        ITaskModelRepository taskRepository,
        ITeamMemberRepository teamMemberRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _taskHandlerRepository = taskHandlerRepository;
        _taskRepository = taskRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<int> Handle(
        CreateTaskHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        // Check if the requested task exists.
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task == null)
        {
            throw new NotFoundException($"Task with id {request.TaskId} was not found.");
        }

        // Check if the user is a team member of the task's project.
        var teamMember = await _teamMemberRepository.GetAsync(
            teamMember =>
                teamMember.UserId == request.UserId && teamMember.ProjectId == task.ProjectId
        );

        if (teamMember == null)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} cannot add any handlers to task with id {request.TaskId}."
            );
        }

        // Check if the target team member exists (the user to be assigned).
        var targetTeamMember = await _teamMemberRepository.GetAsync(
            teamMember =>
                teamMember.UserId == request.UserId && teamMember.ProjectId == task.ProjectId
        );

        if (targetTeamMember == null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} cannot be assigned to task with id {request.TaskId}."
            );
        }

        // Check if the task handler already exists.
        var taskHandler = await _taskHandlerRepository.GetAsync(
            handler => handler.TasksId == request.TaskId && handler.TeamMemberId == task.ProjectId
        );

        if (taskHandler != null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} is already assigned to the task with Id {request.TaskId}."
            );
        }

        // Create a new TaskHandler entity and add it to the repository.
        var taskHandlerToAdd = new TaskHandler()
        {
            TasksId = request.TaskId,
            TeamMemberId = targetTeamMember.Id,
            CreatedAt = DateTime.Now
        };

        var addedTaskHandler = await _taskHandlerRepository.AddAsync(taskHandlerToAdd);
        await _taskHandlerRepository.SaveAsync();

        return addedTaskHandler.Id;
    }
}
