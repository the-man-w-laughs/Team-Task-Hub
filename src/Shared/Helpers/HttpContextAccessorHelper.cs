using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Shared.Helpers
{
    public class HttpContextAccessorHelper
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public HttpContextAccessorHelper(Mock<IHttpContextAccessor> httpContextAccessorMock)
        {
            _httpContextAccessorMock = httpContextAccessorMock;
        }

        public void SetupHttpContextProperty(int userId)
        {
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }
                    )
                )
            };
            _httpContextAccessorMock.SetupProperty(x => x.HttpContext, httpContext);
        }
    }
}
