using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Users.Queries;

public record GetAllUsersQuery() : IRequest<IEnumerable<UserResponseDto>>;
