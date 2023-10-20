using FluentValidation;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.DtoValidators.Utils;
using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.DtoValidators;

public class TaskModelRequestDtoValidator : AbstractValidator<TaskModelRequestDto>
{
    public TaskModelRequestDtoValidator()
    {
        RuleFor(dto => dto.PriorityId)
            .Must(priorityId => Enum.IsDefined(typeof(TaskPriorityEnum), priorityId))
            .WithMessage("Invalid priority value.");

        var maxContentLength = TaskModelConstraints.maxContentLength;
        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(maxContentLength)
            .WithMessage($"Content cannot exceed {maxContentLength} characters.");

        RuleFor(dto => dto.Deadline).SetValidator(new DateTimeValidator());
    }
}
