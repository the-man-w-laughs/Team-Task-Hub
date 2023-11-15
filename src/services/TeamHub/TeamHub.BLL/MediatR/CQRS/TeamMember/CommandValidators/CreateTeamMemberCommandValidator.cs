using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class CreateCommentCommandValidator : AbstractValidator<CreateTeamMemberCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotNull();
        RuleFor(x => x.UserId).NotNull();
    }
}
