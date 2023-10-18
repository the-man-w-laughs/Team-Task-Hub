using Microsoft.AspNetCore.Http;
using MediatR;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.Pipeline;

public class AuthenticationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        if (userId is null)
        {
            throw new AuthorizationException($"User with id {userId} was not found");
        }

        var response = await next();
        return response;
    }
}
