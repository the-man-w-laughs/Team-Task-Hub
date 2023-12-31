using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllTasksCommentsQueryHandler
    : IRequestHandler<GetAllTasksCommentsQuery, IEnumerable<CommentResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly ITaskQueryService _taskService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly ILogger<GetAllTasksCommentsQueryHandler> _logger;

    public GetAllTasksCommentsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository,
        IMapper mapper,
        IUserQueryService userService,
        ITaskQueryService taskService,
        ITeamMemberQueryService teamMemberService,
        ILogger<GetAllTasksCommentsQueryHandler> logger
    )
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _userService = userService;
        _taskService = taskService;
        _teamMemberService = teamMemberService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<CommentResponseDto>> Handle(
        GetAllTasksCommentsQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();
        _logger.LogInformation(
            "User {UserId} attempting to retrieve all task comments for ID {TaskId}.",
            userId,
            request.TaskId
        );

        // check if current user exists
        var user = await _userService.GetExistingUserAsync(userId, cancellationToken);

        // retrieve required task
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);

        // only project members can access to related entities
        var teamMember = await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            task.ProjectId,
            cancellationToken
        );

        // retrieve all the comments
        var taskComments = await _commentRepository.GetAllAsync(
            comment => comment.TasksId == request.TaskId,
            request.Offset,
            request.Limit,
            cancellationToken
        );

        _logger.LogInformation(
            "Retrieved {Count} task comments for ID {TaskId}.",
            taskComments.Count(),
            request.TaskId
        );

        var result = taskComments.Select(comment => _mapper.Map<CommentResponseDto>(comment));

        return result;
    }
}
