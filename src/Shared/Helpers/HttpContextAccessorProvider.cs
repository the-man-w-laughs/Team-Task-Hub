using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Shared.Helpers;

public class HttpContextAccessorProvider
{
    public static Mock<IHttpContextAccessor> MockHttpContextWithUserIdClaim(int userId)
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }
                )
            )
        };
        httpContextAccessorMock.SetupProperty(x => x.HttpContext, httpContext);
        return httpContextAccessorMock;
    }
}
