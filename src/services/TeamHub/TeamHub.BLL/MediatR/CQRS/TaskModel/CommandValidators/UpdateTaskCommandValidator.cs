using FluentValidation;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.TaskId).NotNull();
        RuleFor(x => x.TaskModelRequestDto).SetValidator(new TaskModelRequestDtoValidator());
    }
}
