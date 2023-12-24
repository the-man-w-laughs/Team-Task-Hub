using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly ICommentQueryService _commentService;
    private readonly IUserQueryService _userService;
    private readonly ILogger<DeleteCommentCommandHandler> _logger;

    public DeleteCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository,
        IMapper mapper,
        ICommentQueryService commentService,
        IUserQueryService userService,
        ILogger<DeleteCommentCommandHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _commentRepository = commentRepository;
        _mapper = mapper;
        _commentService = commentService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<CommentResponseDto> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();
        _logger.LogInformation(
            "User with ID {UserId} is attempting to delete a comment with ID {CommentId}.",
            userId,
            request.CommentId
        );

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // check if required comment exists
        var comment = await _commentService.GetExistingCommentAsync(
            request.CommentId,
            cancellationToken
        );

        // only author can delete comment
        if (userId != comment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to delete comment with id {comment.Id}."
            );
        }

        // delete comment
        _commentRepository.Delete(comment);
        await _commentRepository.SaveAsync(cancellationToken);

        _logger.LogInformation("Comment with ID {CommentId} deleted successfully.", comment.Id);

        var result = _mapper.Map<CommentResponseDto>(comment);

        return result;
    }
}
