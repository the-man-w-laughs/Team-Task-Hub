using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public record CreateCommentCommand(int TaskId, CommentRequestDto CommentRequestDto) : IRequest<int>;
