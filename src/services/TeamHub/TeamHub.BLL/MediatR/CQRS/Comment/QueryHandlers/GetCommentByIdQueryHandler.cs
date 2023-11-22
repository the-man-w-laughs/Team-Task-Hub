using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly ITeamMemberService _teamMemberService;

    public GetCommentByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserService userService,
        ICommentService commentService,
        ITeamMemberService teamMemberService
    )
    {
        _mapper = mapper;
        _userService = userService;
        _commentService = commentService;
        _teamMemberService = teamMemberService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentResponseDto> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // get required comment
        var comment = await _commentService.GetCommentAsync(request.CommentId, cancellationToken);
        var response = _mapper.Map<CommentResponseDto>(comment);

        // only project members have access to related entities
        var projectId = comment.Task.ProjectId;
        await _teamMemberService.GetTeamMemberAsync(userId, projectId, cancellationToken);

        return response;
    }
}
