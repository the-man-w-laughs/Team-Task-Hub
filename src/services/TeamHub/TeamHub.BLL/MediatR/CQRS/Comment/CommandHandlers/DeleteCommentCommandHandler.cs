using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _commentRepository = commentRepository;
    }

    public async Task<int> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (comment == null)
        {
            throw new NotFoundException($"Cannot find comment with id {request.CommentId}");
        }

        if (userId != comment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot delete comment with id {comment.Id}."
            );
        }

        _commentRepository.Delete(comment);
        await _commentRepository.SaveAsync();

        return comment.Id;
    }
}
