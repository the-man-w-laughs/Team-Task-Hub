using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public record UpdateCommentCommand(int CommentId, CommentRequestDto CommentRequestDto)
    : IRequest<CommentResponseDto>;
