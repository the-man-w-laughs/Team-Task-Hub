using FluentValidation;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.CommentRequestDto).SetValidator(new CommentRequestDtoValidator());
    }
}
