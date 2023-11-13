using System.Reflection;
using MassTransit;

namespace Identity.WebAPI.Extensions
{
    public static class MessageBrokerExtensions
    {
        public static void ConfigureMassTransit(
            this IServiceCollection services,
            ConfigurationManager config
        )
        {
            services.AddMassTransit(x =>
            {
                var assembly = Assembly.GetEntryAssembly();
                var host = config["MessageBroker:Host"];
                var virtualHost = config["MessageBroker:VirtualHost"];
                var username = config["MessageBroker:Username"];
                var password = config["MessageBroker:Password"];

                x.AddConsumers(assembly);

                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        cfg.Host(
                            host,
                            virtualHost,
                            h =>
                            {
                                h.Username(username);
                                h.Password(password);
                            }
                        );

                        cfg.ConfigureEndpoints(context);
                    }
                );
            });
        }
    }
}
