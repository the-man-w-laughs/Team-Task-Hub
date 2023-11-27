using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using Shared.gRPC;

namespace ReportHub.BLL.Extensions
{
    public static class GrpcRegistrationExtensions
    {
        public static void RegisterGrpcClient(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            var endpoint = config["TeamHubConfig:EndPoint"];
            var channel = GrpcChannel.ForAddress(endpoint);

            services.AddSingleton(channel);

            services.AddScoped<IFullProjectInfoService>(provider =>
            {
                var client = channel.CreateGrpcService<IFullProjectInfoService>();

                return client;
            });
        }
    }
}
