using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, int?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCommentCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int?> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        return 0;
    }
}
