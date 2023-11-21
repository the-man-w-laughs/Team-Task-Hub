using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using Shared.Exceptions;
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

        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

        if (task == null)
        {
            throw new NotFoundException($"Task with id {request.TaskId} was not found.");
        }

        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(
            userId,
            task.ProjectId,
            cancellationToken
        );

        if (teamMember == null)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have access to project with id {task.ProjectId}."
            );
        }

        var commentToAdd = _mapper.Map<Comment>(request.CommentRequestDto);

        commentToAdd.AuthorId = userId;
        commentToAdd.TasksId = request.TaskId;

        var addedComment = await _commentRepository.AddAsync(commentToAdd, cancellationToken);
        await _commentRepository.SaveAsync(cancellationToken);

        return addedComment.Id;
    }
}
