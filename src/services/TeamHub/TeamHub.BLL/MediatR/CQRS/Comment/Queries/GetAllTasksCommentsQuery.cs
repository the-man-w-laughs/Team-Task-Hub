using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Queries;

public record GetAllTasksCommentsQuery(int TaskId) : IRequest<IEnumerable<CommentResponseDto>>;
