using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskRepository;
    private readonly IUserQueryService _userService;
    private readonly ITaskQueryService _taskService;
    private readonly ILogger<UpdateTaskCommandHandler> _logger;

    public UpdateTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskRepository,
        IUserQueryService userService,
        ITaskQueryService taskService,
        ILogger<UpdateTaskCommandHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskRepository = taskRepository;
        _userService = userService;
        _taskService = taskService;
        _logger = logger;
    }

    public async Task<TaskModelResponseDto> Handle(
        UpdateTaskCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation(
            "User {UserId} is trying to update task with id {TaskId}.",
            userId,
            request.TaskId
        );

        // Check if the user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // Check if the requested task exists
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);

        // only task author can update it
        if (userId != task.AuthorTeamMember.UserId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesnt have rights to alter comment with id {task.Id}."
            );
        }

        // update task
        _mapper.Map(request.TaskModelRequestDto, task);
        _taskRepository.Update(task);
        await _taskRepository.SaveAsync(cancellationToken);

        var result = _mapper.Map<TaskModelResponseDto>(task);

        _logger.LogInformation("Successfully updated task with id {TaskId}.", request.TaskId);

        return result;
    }
}
