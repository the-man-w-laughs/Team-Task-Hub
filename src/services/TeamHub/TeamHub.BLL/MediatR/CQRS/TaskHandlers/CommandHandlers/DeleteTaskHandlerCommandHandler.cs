using Amazon.Runtime.Internal.Util;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TaskHandler;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class DeleteTaskHandlerCommandHandler
    : IRequestHandler<DeleteTaskHandlerCommand, TaskHandlerResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly IUserQueryService _userService;
    private readonly ITaskQueryService _taskService;
    private readonly ITaskHandlerRepository _taskHandlerRepository;
    private readonly ILogger<DeleteTaskHandlerCommandHandler> _logger;

    public DeleteTaskHandlerCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITeamMemberQueryService teamMemberService,
        IUserQueryService userService,
        ITaskQueryService taskService,
        ITaskHandlerRepository taskHandlerRepository,
        ILogger<DeleteTaskHandlerCommandHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _taskService = taskService;
        _taskHandlerRepository = taskHandlerRepository;
        _logger = logger;
    }

    public async Task<TaskHandlerResponseDto> Handle(
        DeleteTaskHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();
        _logger.LogInformation(
            "User {UserId} attempting to unassign task with ID {TaskId} from user with ID {AssigneeId}.",
            userId,
            request.TaskId,
            request.UserId
        );

        // Check if the current user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);
        // Check if the target user exists.
        await _userService.GetExistingUserAsync(request.UserId, cancellationToken);

        // Check if the task exists.
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);

        // Check if the current users team member exists.
        await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            task.ProjectId,
            cancellationToken
        );

        // Check if the target team member exists (the user to be assigned).
        var targetTeamMember = await _teamMemberService.GetExistingTeamMemberAsync(
            request.UserId,
            task.ProjectId,
            cancellationToken
        );

        // Check if the task handler already exists.
        var taskHandler = await _taskHandlerRepository.GetTaskHandlerAsync(
            targetTeamMember.Id,
            request.TaskId,
            cancellationToken
        );

        if (taskHandler == null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} is not assigned to the task with Id {request.TaskId}."
            );
        }

        _taskHandlerRepository.Delete(taskHandler);
        await _taskHandlerRepository.SaveAsync(cancellationToken);

        var result = _mapper.Map<TaskHandlerResponseDto>(taskHandler);
        _logger.LogInformation(
            "Task with ID {TaskId} unassigned from user with ID {AssigneeId} successfully by user {UserId}.",
            request.TaskId,
            request.UserId,
            userId
        );

        return result;
    }
}
