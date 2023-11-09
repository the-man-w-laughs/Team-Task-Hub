using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddMassTransit(x =>
            {
                var assembly = Assembly.GetAssembly(typeof(UserCreatedMessageConsumer));
                var host = config["RabbitMQ:Host"];
                var virtualHost = config["RabbitMQ:VirtualHost"];
                var username = config["RabbitMQ:Username"];
                var password = config["RabbitMQ:Password"];

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

                        cfg.ReceiveEndpoint(
                            config["RabbitMQ:ReceiveEndpoints:UserCreated"],
                            x =>
                            {
                                x.Bind<UserCreatedMessage>();
                                x.ConfigureConsumer<UserCreatedMessageConsumer>(context);
                            }
                        );

                        cfg.ReceiveEndpoint(
                            config["RabbitMQ:ReceiveEndpoints:UserDeleted"],
                            x =>
                            {
                                x.Bind<UserDeletedMessage>();
                                x.ConfigureConsumer<UserDeletedMessageConsumer>(context);
                            }
                        );
                    }
                );
            });
        }
    }
}
