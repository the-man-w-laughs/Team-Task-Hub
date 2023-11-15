using FluentValidation;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteTeamMemberCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.ProjectId).NotNull();
    }
}
