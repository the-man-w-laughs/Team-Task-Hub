using Amazon.Runtime.Internal.Util;
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

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserQueryService _userService;
    private readonly ICommentQueryService _commentService;
    private readonly ILogger<UpdateCommentCommandHandler> _logger;

    public UpdateCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ICommentRepository commentRepository,
        IUserQueryService userService,
        ICommentQueryService commentService,
        ILogger<UpdateCommentCommandHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _userService = userService;
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<CommentResponseDto> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();
        _logger.LogInformation(
            "User with ID {UserId} is attempting to update a comment with ID {CommentId}.",
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

        // only author can update comment
        if (userId != comment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter comment with id {comment.Id}."
            );
        }

        // update comment
        var createdAt = comment.CreatedAt;
        _mapper.Map(request.CommentRequestDto, comment);
        comment.CreatedAt = createdAt;
        _commentRepository.Update(comment);
        await _commentRepository.SaveAsync(cancellationToken);

        _logger.LogInformation("Comment with ID {CommentId} updated successfully.", comment.Id);

        var result = _mapper.Map<CommentResponseDto>(comment);

        return result;
    }
}
