using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly ITaskQueryService _taskService;
    private readonly ILogger<GetTaskByIdQueryHandler> _logger;

    public GetTaskByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserQueryService userService,
        ITeamMemberQueryService teamMemberService,
        ITaskQueryService taskService,
        ILogger<GetTaskByIdQueryHandler> logger
    )
    {
        _mapper = mapper;
        _userService = userService;
        _teamMemberService = teamMemberService;
        _taskService = taskService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TaskModelResponseDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation(
            "User {UserId} is trying to retrieve task with id {TaskId}.",
            userId,
            request.TaskId
        );

        // Check if the user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get requested task
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);
        var response = _mapper.Map<TaskModelResponseDto>(task);

        // only team member has access to related entities
        await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            task.ProjectId,
            cancellationToken
        );

        _logger.LogInformation("Successfully retrieved task with id {TaskId}.", request.TaskId);

        return response;
    }
}
