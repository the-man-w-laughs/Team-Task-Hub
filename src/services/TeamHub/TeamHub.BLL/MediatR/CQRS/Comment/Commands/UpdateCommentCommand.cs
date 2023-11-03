using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public record UpdateCommentCommand(int CommentId, CommentRequestDto CommentRequestDto)
    : IRequest<int>;
