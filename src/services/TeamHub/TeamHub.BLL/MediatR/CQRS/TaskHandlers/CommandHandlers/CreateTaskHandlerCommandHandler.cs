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
        var userId = _httpContextAccessor.GetUserId();

        // Check if the requested task exists and current users team member exists.
        var task = await _taskRepository.GetTaskByIdAsync(request.TaskId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, task.ProjectId);

        // Check if the target team member exists (the user to be assigned).
        var targetTeamMember = await _teamMemberRepository.GetTeamMemberAsync(
            request.UserId,
            task.ProjectId
        );

        // Check if the task handler already exists.
        var taskHandler = await _taskHandlerRepository.GetTaskHandlerAsync(
            targetTeamMember.Id,
            request.TaskId
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
