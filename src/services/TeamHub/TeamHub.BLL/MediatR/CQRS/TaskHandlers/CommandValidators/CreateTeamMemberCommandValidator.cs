using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class CreateCommentCommandValidator : AbstractValidator<CreateTaskHandlerCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.TaskId).NotNull();
        RuleFor(x => x.UserId).NotNull();
    }
}
