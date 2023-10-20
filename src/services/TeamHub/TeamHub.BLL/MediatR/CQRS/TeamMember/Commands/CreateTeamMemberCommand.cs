using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public record CreateTeamMemberCommand(int ProjectId, int UserId) : IRequest<int>;
