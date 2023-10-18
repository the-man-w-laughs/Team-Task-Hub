using FluentValidation;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.DtoValidators;

public class TeamMemberRequestDtoValidator : AbstractValidator<TeamMemberRequestDto>
{
    public TeamMemberRequestDtoValidator() { }
}
