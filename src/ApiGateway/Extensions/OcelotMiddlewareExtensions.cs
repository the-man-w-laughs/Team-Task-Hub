using Ocelot.Middleware;
using Shared.Repository.NoSql.Redis;
using Shared.Extensions;

namespace ApiGateway.Extensions
{
    public static class OcelotMiddlewareExtensions
    {
        public static OcelotPipelineConfiguration AddUserCacheMiddleware(
            this OcelotPipelineConfiguration configuration
        )
        {
            configuration.AuthorizationMiddleware = async (ctx, next) =>
            {
                IHttpContextAccessor httpContextAccessor =
                    ctx.RequestServices.GetRequiredService<IHttpContextAccessor>();
                IUserRequestRepository userRequestRepository =
                    ctx.RequestServices.GetRequiredService<IUserRequestRepository>();

                try
                {
                    int userId = httpContextAccessor.GetUserId();
                    userRequestRepository.SetLatestRequestTime(userId.ToString(), DateTime.Now);
                }
                finally
                {
                    await next.Invoke();
                }
            };

            return configuration;
        }
    }
}
