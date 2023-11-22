using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ITeamMemberService _teamMemberService;
    private readonly ITaskService _taskService;

    public GetTaskByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserService userService,
        ITeamMemberService teamMemberService,
        ITaskService taskService
    )
    {
        _mapper = mapper;
        _userService = userService;
        _teamMemberService = teamMemberService;
        _taskService = taskService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TaskModelResponseDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the user exists.
        await _userService.GetUserAsync(userId, cancellationToken);

        // get requested task
        var task = await _taskService.GetTaskAsync(request.TaskId, cancellationToken);
        var response = _mapper.Map<TaskModelResponseDto>(task);

        // only team member has access to related entities
        await _teamMemberService.GetTeamMemberAsync(userId, task.ProjectId, cancellationToken);

        return response;
    }
}
