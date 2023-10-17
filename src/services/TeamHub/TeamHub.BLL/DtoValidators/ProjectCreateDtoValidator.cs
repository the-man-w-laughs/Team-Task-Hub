using FluentValidation;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.DtoValidators;

public class ProjectCreateDtoValidator : AbstractValidator<CommentCreateDto>
{
    public ProjectCreateDtoValidator()
    {
        var maxNameLength = ProjectConstraints.maxNameLength;
        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(maxNameLength)
            .WithMessage($"Name cannot exceed {maxNameLength} characters.");
    }
}
