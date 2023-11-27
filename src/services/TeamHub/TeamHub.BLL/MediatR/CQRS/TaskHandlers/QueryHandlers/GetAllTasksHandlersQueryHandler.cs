using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Queries;

public class GetAllProjectsTeamMembersQueryHandler
    : IRequestHandler<GetAllTaskHandlersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskHandlerRepository _taskHandlerRepository;
    private readonly ITaskQueryService _taskService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly IUserQueryService _userService;

    public GetAllProjectsTeamMembersQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskHandlerRepository taskHandlerRepository,
        ITaskQueryService taskService,
        ITeamMemberQueryService teamMemberService,
        IUserQueryService userService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskHandlerRepository = taskHandlerRepository;
        _taskService = taskService;
        _teamMemberService = teamMemberService;
        _userService = userService;
    }

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllTaskHandlersQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // Check if the requested task exists and current users team member exists.
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);

        // Check if user has access to the project
        var teamMember = await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            task.ProjectId,
            cancellationToken
        );

        // get all task handlers
        var taskHandlers = await _taskHandlerRepository.GetAllAsync(
            teamMember => teamMember.TaskId == request.TaskId,
            request.Offset,
            request.Limit,
            cancellationToken
        );
        var usersResponseDto = taskHandlers.Select(
            taskHandler => _mapper.Map<UserResponseDto>(taskHandler.TeamMember.User)
        );

        return usersResponseDto;
    }
}
