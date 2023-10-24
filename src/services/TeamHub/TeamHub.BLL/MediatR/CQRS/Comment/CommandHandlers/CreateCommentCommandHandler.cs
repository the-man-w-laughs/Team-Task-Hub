using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly ITaskModelRepository _taskRepository;

    public CreateCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ICommentRepository commentRepository,
        ITeamMemberRepository teamMemberRepository,
        ITaskModelRepository taskRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _teamMemberRepository = teamMemberRepository;
        _taskRepository = taskRepository;
    }

    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();

        var task = await _taskRepository.GetTaskByIdAsync(request.TaskId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, task.ProjectId);

        var commentToAdd = _mapper.Map<Comment>(request.CommentRequestDto);

        commentToAdd.AuthorId = userId;
        commentToAdd.TasksId = request.TaskId;
        commentToAdd.CreatedAt = DateTime.Now;

        var addedComment = await _commentRepository.AddAsync(commentToAdd);
        await _commentRepository.SaveAsync();

        return addedComment.Id;
    }
}
