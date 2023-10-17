using System.Security.Claims;

namespace TeamHub.BLL.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst(x => x.Type.Equals("sub"));
        if (claim == null)
        {
            throw new InvalidOperationException("The 'sub' claim is not present in the principal.");
        }

        return int.Parse(claim.Value);
    }
}
