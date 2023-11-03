using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.TaskId).NotNull();
    }
}
