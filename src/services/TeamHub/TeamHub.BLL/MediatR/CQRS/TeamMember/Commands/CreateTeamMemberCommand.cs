using MediatR;
using TeamHub.BLL.Dtos.TeamMember;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public record CreateTeamMemberCommand(int ProjectId, int UserId) : IRequest<TeamMemberResponseDto>;
