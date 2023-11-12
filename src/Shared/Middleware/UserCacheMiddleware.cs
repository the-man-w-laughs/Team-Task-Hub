using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using Shared.Repository.NoSql.Redis;

namespace ApiGateway.Middlewares
{
    public class UserCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserRequestRepository _userRequestRepository;

        public UserCacheMiddleware(
            RequestDelegate next,
            IUserRequestRepository userRequestRepository
        )
        {
            _next = next;
            _userRequestRepository = userRequestRepository;
        }

        public async Task InvokeAsync(HttpContext context, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                int userId = httpContextAccessor.GetUserId();
                _userRequestRepository.SetLatestRequestTime(userId.ToString(), DateTime.Now);
            }
            finally
            {
                await _next(context);
            }
        }
    }
}
