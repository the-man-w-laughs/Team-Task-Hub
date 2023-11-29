using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public record DeleteCommentCommand(int CommentId) : IRequest<CommentResponseDto>;
