using IdentityServer4.Services;

namespace Identity.WebAPI.Extensions
{
    public static class CorsExtensions
    {
        public static void ConfigureCors(this IServiceCollection services, IWebHostEnvironment environment)
        {
            //if (!environment.IsProduction())
            //{
            //    services.AddSingleton<ICorsPolicyService>((container) => {
            //        var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
            //        return new DefaultCorsPolicyService(logger)
            //        {
            //            AllowAll = true
            //        };
            //    });
            //}
        }
    }
}
