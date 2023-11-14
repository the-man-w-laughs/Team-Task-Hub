using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.SharedModels;
using TeamHub.BLL.MassTransit.Consumers;

namespace TeamHub.BLL.Extensions
{
    public static class MassTransitExtensions
    {
        public static void ConfigureMassTransit(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQ"));

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetEndpointNameFormatter(
                    new KebabCaseEndpointNameFormatter(prefix: "teamhub", includeNamespace: false)
                );

                busConfigurator.AddConsumer<UserCreatedMessageConsumer>();
                busConfigurator.AddConsumer<UserDeletedMessageConsumer>();

                busConfigurator.UsingRabbitMq(
                    (busRegistrationContext, busConfigurator) =>
                    {
                        var settings = busRegistrationContext
                            .GetRequiredService<IOptions<RabbitMQSettings>>()
                            .Value;

                        busConfigurator.Host(
                            new Uri(settings.Host),
                            hostConfigurator =>
                            {
                                hostConfigurator.Username(settings.Username);
                                hostConfigurator.Password(settings.Password);
                            }
                        );

                        busConfigurator.ConfigureEndpoints(busRegistrationContext);
                    }
                );
            });
        }
    }
}
