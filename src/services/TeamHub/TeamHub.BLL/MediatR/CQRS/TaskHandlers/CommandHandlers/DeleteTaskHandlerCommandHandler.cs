using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class DeleteTaskHandlerCommandHandler : IRequestHandler<DeleteTaskHandlerCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskHandlerRepository _taskHandlerRepository;
    private readonly ITaskModelRepository _taskRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public DeleteTaskHandlerCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskHandlerRepository taskHandlerRepository,
        ITaskModelRepository taskRepository,
        ITeamMemberRepository teamMemberRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskHandlerRepository = taskHandlerRepository;
        _taskRepository = taskRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<int> Handle(
        DeleteTaskHandlerCommand request,
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

        if (taskHandler == null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} is already assigned to the task with Id {request.TaskId}."
            );
        }

        _taskHandlerRepository.Delete(taskHandler);
        await _taskHandlerRepository.SaveAsync();

        return taskHandler.TeamMember.UserId;
    }
}
