using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCommentCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentResponseDto> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        return new CommentResponseDto();
    }
}
