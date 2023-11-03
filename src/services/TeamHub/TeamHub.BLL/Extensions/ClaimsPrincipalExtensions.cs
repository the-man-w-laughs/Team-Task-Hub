using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TeamHub.BLL.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var principal = httpContextAccessor?.HttpContext?.User;

        if (principal == null)
        {
            throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        return principal.GetUserId();
    }

    public static int GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier));

        if (claim == null)
        {
            throw new InvalidOperationException("The 'sub' claim is not present in the principal.");
        }

        return int.Parse(claim.Value);
    }
}
