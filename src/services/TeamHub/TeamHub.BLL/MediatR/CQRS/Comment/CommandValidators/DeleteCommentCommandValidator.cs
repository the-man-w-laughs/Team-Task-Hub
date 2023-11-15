using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotNull();
    }
}
