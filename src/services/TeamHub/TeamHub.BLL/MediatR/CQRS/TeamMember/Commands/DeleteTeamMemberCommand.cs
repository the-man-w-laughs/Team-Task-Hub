using MediatR;
using TeamHub.BLL.Dtos.TeamMember;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public record DeleteTeamMemberCommand(int ProjectId, int UserId) : IRequest<TeamMemberResponseDto>;
