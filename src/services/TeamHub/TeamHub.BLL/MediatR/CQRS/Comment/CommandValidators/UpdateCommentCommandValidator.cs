using FluentValidation;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotNull();
        RuleFor(x => x.CommentRequestDto).SetValidator(new CommentRequestDtoValidator());
    }
}
