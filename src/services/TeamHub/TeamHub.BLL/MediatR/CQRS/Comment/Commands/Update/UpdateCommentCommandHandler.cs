using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateCommentCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentResponseDto> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        return new CommentResponseDto();
    }
}
