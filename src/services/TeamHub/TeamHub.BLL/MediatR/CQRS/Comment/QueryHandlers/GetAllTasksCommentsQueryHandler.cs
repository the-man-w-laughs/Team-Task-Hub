using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllTasksCommentsQueryHandler
    : IRequestHandler<GetAllTasksCommentsQuery, IEnumerable<CommentResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly ITaskModelRepository _taskRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetAllTasksCommentsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository,
        ITaskModelRepository taskModelRepository,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper
    )
    {
        _commentRepository = commentRepository;
        _taskRepository = taskModelRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<CommentResponseDto>> Handle(
        GetAllTasksCommentsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        var task = await _taskRepository.GetTaskByIdAsync(request.TaskId, cancellationToken);
        await _teamMemberRepository.GetTeamMemberAsync(userId, task.ProjectId, cancellationToken);

        var taskComments = await _commentRepository.GetAllAsync(
            comment => comment.TasksId == request.TaskId,
            cancellationToken
        );

        var projectResponseDtos = taskComments.Select(
            project => _mapper.Map<CommentResponseDto>(project)
        );

        return projectResponseDtos;
    }
}
