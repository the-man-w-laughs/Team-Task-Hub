using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly ICommentQueryService _commentService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly ILogger<GetCommentByIdQueryHandler> _logger;

    public GetCommentByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserQueryService userService,
        ICommentQueryService commentService,
        ITeamMemberQueryService teamMemberService,
        ILogger<GetCommentByIdQueryHandler> logger
    )
    {
        _mapper = mapper;
        _userService = userService;
        _commentService = commentService;
        _teamMemberService = teamMemberService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentResponseDto> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();
        _logger.LogInformation(
            "User with ID {UserId} is attempting to retrieve comment with ID {CommentId}.",
            userId,
            request.CommentId
        );

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get required comment
        var comment = await _commentService.GetExistingCommentAsync(
            request.CommentId,
            cancellationToken
        );
        var response = _mapper.Map<CommentResponseDto>(comment);

        // only project members have access to related entities
        var projectId = comment.Task.ProjectId;
        await _teamMemberService.GetExistingTeamMemberAsync(userId, projectId, cancellationToken);

        _logger.LogInformation(
            "Successfully retrieved comment with ID {CommentId}.",
            request.CommentId
        );

        return response;
    }
}
