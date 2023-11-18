using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Queries;

public record GetAllProjectsTeamMembersQuery(int ProjectId, int Offset, int Limit)
    : IRequest<IEnumerable<UserResponseDto>>;
