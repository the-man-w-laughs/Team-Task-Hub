using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public record DeleteCommentCommand(int id) : IRequest<int?>;
