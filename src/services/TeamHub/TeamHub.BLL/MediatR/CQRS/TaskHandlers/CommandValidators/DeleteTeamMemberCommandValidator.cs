using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteTaskHandlerCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.TaskId).NotNull();
    }
}
