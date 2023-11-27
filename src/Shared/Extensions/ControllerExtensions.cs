using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers(
                opts =>
                    opts.Conventions.Add(
                        new RouteTokenTransformerConvention(new ToKebabParameterTransformer())
                    )
            );

            return services;
        }
    }

    public class ToKebabParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object? value) => value?.ToString()?.ToKebabCase();
    }
}
