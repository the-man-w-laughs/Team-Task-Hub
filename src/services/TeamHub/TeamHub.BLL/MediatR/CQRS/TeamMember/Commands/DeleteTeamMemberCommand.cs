using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public record DeleteTeamMemberCommand(int ProjectId, int UserId) : IRequest<int>;
