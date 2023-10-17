using FluentValidation;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.DtoValidators.Utils;
using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.DtoValidators;

public class TaskModelCreateDtoValidator : AbstractValidator<TaskModelCreateDto>
{
    public TaskModelCreateDtoValidator()
    {
        RuleFor(dto => dto.ProjectsId).NotEmpty().WithMessage("Project ID is required.");

        RuleFor(dto => dto.PriorityId).IsInEnum().WithMessage("Invalid priority value.");

        var maxContentLength = TaskModelConstraints.maxContentLength;
        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(maxContentLength)
            .WithMessage($"Content cannot exceed {maxContentLength} characters.");

        RuleFor(dto => dto.Deadline).SetValidator(new DateTimeValidator());
    }
}
