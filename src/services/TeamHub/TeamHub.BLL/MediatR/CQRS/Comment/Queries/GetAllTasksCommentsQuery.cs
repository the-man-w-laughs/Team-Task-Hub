using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Queries;

public record GetAllTasksCommentsQuery(int TasksId) : IRequest<IEnumerable<CommentResponseDto>>;
