using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ConfigurationSectionExtensions
{
    public static IServiceCollection AddConfigurationSection<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null
    )
        where T : class
    {
        var name = sectionName ?? typeof(T).Name;
        var section = configuration.GetSection(name);
        services.Configure<T>(section);
        return services;
    }
}
