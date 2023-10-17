using FluentValidation;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.DtoValidators;

public class TeamMemberCreateDtoValidator : AbstractValidator<TeamMemberCreateDto>
{
    public TeamMemberCreateDtoValidator()
    {
        RuleFor(dto => dto.UsersId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(dto => dto.ProjectsId).NotEmpty().WithMessage("Project ID is required.");
    }
}
