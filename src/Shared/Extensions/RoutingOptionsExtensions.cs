using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class RoutingOptionsExtensions
    {
        public static void AddRoutingOptions(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}
