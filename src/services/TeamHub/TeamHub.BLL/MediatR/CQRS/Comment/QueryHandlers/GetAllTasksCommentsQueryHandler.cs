using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllTasksCommentsQueryHandler
    : IRequestHandler<GetAllTasksCommentsQuery, IEnumerable<CommentResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetAllTasksCommentsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository,
        IMapper mapper
    )
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<CommentResponseDto>> Handle(
        GetAllTasksCommentsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var taskComments = await _commentRepository.GetAllAsync(
            comment => comment.TasksId == request.TasksId
        );

        var projectResponseDtos = taskComments.Select(
            project => _mapper.Map<CommentResponseDto>(project)
        );

        return projectResponseDtos;
    }
}
