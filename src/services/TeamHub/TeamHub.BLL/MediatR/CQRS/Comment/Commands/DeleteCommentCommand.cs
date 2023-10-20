using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public record DeleteCommentCommand(int CommentId) : IRequest<int>;
