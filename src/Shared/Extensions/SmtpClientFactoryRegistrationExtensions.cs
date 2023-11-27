using Microsoft.Extensions.DependencyInjection;
using Shared.SharedModels;
using Shared.SharedModels.Contracts;

namespace Shared.Extensions;

public static class CustomSmtpClientFactoryRegistrationExtensions
{
    public static IServiceCollection AddSmtpClientFactory(this IServiceCollection services)
    {
        services.AddTransient<ISmtpClientFactory, CustomSmtpClientFactory>();
        return services;
    }
}
