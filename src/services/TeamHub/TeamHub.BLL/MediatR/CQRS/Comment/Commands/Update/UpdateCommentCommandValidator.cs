using FluentValidation;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.CommentRequestDto).SetValidator(new CommentRequestDtoValidator());
    }
}
