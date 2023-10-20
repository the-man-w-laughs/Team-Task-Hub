using FluentValidation;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectRequestDto).SetValidator(new ProjectRequestDtoValidator());
    }
}
