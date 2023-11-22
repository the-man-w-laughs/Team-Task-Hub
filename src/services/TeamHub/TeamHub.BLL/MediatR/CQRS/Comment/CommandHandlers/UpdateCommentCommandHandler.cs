using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;

    public UpdateCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ICommentRepository commentRepository,
        IUserService userService,
        ICommentService commentService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _userService = userService;
        _commentService = commentService;
    }

    public async Task<CommentResponseDto> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // check if required comment exists
        var comment = await _commentService.GetCommentAsync(request.CommentId, cancellationToken);

        // only author can update comment
        if (userId != comment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter comment with id {comment.Id}."
            );
        }

        // update comment
        _mapper.Map(request.CommentRequestDto, comment);
        _commentRepository.Update(comment);
        await _commentRepository.SaveAsync(cancellationToken);
        var result = _mapper.Map<CommentResponseDto>(comment);

        return result;
    }
}
