using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public record CreateCommentCommand(int TaskId, CommentRequestDto CommentCreateDto)
    : IRequest<CommentResponseDto>;
