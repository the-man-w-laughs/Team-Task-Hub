using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class DeletePeojectCommandHandler : IRequestHandler<DeleteProjectCommand, int?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeletePeojectCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int?> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        return 0;
    }
}
