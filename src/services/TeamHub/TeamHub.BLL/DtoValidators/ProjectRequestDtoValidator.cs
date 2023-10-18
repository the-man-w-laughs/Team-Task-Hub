using FluentValidation;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.DtoValidators;

public class ProjectRequestDtoValidator : AbstractValidator<ProjectRequestDto>
{
    public ProjectRequestDtoValidator()
    {
        var maxNameLength = ProjectConstraints.maxNameLength;
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(maxNameLength)
            .WithMessage($"Name cannot exceed {maxNameLength} characters.");
    }
}
