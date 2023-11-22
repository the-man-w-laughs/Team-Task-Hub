using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserService _userService;
    private readonly ITaskService _taskService;
    private readonly ITeamMemberService _teamMemberService;

    public CreateCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ICommentRepository commentRepository,
        IUserService userService,
        ITaskService taskService,
        ITeamMemberService teamMemberService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _userService = userService;
        _taskService = taskService;
        _teamMemberService = teamMemberService;
    }

    public async Task<CommentResponseDto> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // check if required task exists
        var task = await _taskService.GetTaskAsync(request.TaskId, cancellationToken);

        // check if user has access to this task
        await _teamMemberService.GetTeamMemberAsync(userId, task.ProjectId, cancellationToken);

        // create new comment
        var commentToAdd = _mapper.Map<Comment>(request.CommentRequestDto);
        commentToAdd.AuthorId = userId;
        commentToAdd.TasksId = request.TaskId;
        var addedComment = await _commentRepository.AddAsync(commentToAdd, cancellationToken);
        await _commentRepository.SaveAsync(cancellationToken);
        var result = _mapper.Map<CommentResponseDto>(addedComment);

        return result;
    }
}
