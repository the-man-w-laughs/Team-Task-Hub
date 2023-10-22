using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper
    )
    {
        _commentRepository = commentRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentResponseDto> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, comment.Task.ProjectId);

        var response = _mapper.Map<CommentResponseDto>(comment);

        return response;
    }
}
