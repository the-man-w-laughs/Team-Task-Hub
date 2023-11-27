using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskRepository;
    private readonly ITaskQueryService _taskService;
    private readonly IUserQueryService _userService;

    public DeleteTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskModelRepository,
        ITaskQueryService taskService,
        IUserQueryService userService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskRepository = taskModelRepository;
        _taskService = taskService;
        _userService = userService;
    }

    public async Task<TaskModelResponseDto> Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // Check if the requested task exists
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);

        // only task author and project author can delete the task
        if (userId != task.AuthorTeamMember.UserId && userId != task.Project.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} can't delete tesk with id {task.Id}."
            );
        }

        // delete task
        _taskRepository.Delete(task);
        await _taskRepository.SaveAsync(cancellationToken);

        var result = _mapper.Map<TaskModelResponseDto>(task);

        return result;
    }
}
