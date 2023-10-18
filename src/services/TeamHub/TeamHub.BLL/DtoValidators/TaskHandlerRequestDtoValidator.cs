using FluentValidation;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.DtoValidators;

public class TaskHandlerRequestDtoValidator : AbstractValidator<TasksHandlerRequestDto>
{
    public TaskHandlerRequestDtoValidator()
    {
        RuleFor(dto => dto.TeamMembersId).NotEmpty().WithMessage("Team member ID is required.");
        RuleFor(dto => dto.TasksId).NotEmpty().WithMessage("Task ID is required.");
    }
}
