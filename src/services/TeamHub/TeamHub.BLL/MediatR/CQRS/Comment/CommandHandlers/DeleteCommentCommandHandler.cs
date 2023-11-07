using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
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
        var userId = _httpContextAccessor.GetUserId();

        var comment = await _commentRepository.GetCommentByIdAsync(
            request.CommentId,
            cancellationToken
        );

        if (userId != comment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter comment with id {comment.Id}."
            );
        }

        _commentRepository.Delete(comment);
        await _commentRepository.SaveAsync(cancellationToken);

        return comment.Id;
    }
}
