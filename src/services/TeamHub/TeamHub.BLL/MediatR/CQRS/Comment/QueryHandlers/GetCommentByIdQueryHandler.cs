using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
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

        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (comment == null)
        {
            throw new NotFoundException($"Cannot find comment with id {request.CommentId}");
        }

        var projectId = comment.Task.ProjectId;
        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(userId, projectId);

        if (teamMember == null)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have access to project with id {projectId}."
            );
        }

        var response = _mapper.Map<CommentResponseDto>(comment);

        return response;
    }
}
