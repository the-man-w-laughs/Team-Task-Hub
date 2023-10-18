using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.id).NotNull();
    }
}
