using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ConfigurationSectionExtensions
{
    public static IServiceCollection AddConfigurationSection<T>(
        this IServiceCollection services,
        IConfiguration configuration
    )
        where T : class
    {
        var name = typeof(T).Name;
        var section = configuration.GetSection(name);
        services.Configure<T>(section);
        return services;
    }
}
