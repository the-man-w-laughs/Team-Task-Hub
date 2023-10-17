using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.CQRS.Comment.Commands;

public class AddWordCollectionCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentDto> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        return new CommentDto();
    }
}
