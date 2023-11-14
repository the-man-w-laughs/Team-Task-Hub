using MassTransit;
using Shared.SharedModels;
using Microsoft.Extensions.Options;

namespace Identity.WebAPI.Extensions
{
    public static class MessageBrokerExtensions
    {
        public static void ConfigureMassTransit(
            this IServiceCollection services,
            ConfigurationManager config
        )
        {
            services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQ"));

            services.AddMassTransit(busConfigurator =>
            {
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
                    }
                );
            });
        }
    }
}
