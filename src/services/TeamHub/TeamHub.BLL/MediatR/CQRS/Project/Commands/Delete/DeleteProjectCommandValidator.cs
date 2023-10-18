using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.id).NotNull();
    }
}
