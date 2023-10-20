using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Queries;

public record GetCommentByIdQuery(int commentId) : IRequest<CommentResponseDto>;
