using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Users.Queries;

public record GetAllUsersQuery(int Limit, int Offset) : IRequest<IEnumerable<UserResponseDto>>;
