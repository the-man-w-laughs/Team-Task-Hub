using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
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
