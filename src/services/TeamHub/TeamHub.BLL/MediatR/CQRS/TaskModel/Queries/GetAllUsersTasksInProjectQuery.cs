using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public record GetAllUsersTasksInProjectQuery(int commentId) : IRequest<CommentResponseDto>;
