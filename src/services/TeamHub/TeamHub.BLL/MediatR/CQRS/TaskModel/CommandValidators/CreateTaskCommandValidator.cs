using FluentValidation;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class CreateCommentCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.TaskModelRequestDto).SetValidator(new TaskModelRequestDtoValidator());
    }
}
