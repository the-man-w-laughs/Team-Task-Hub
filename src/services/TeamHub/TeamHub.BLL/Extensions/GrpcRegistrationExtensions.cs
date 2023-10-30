using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Server;
using TeamHub.BLL.gRPC;

namespace TeamHub.BLL
{
    public static class GrpcRegistrationExtensions
    {
        public static void ReristerRrpcService(this IServiceCollection services)
        {
            services.AddCodeFirstGrpc();
        }

        public static WebApplicationBuilder ConfigureWebHost(this WebApplicationBuilder builder)
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(
                    IPAddress.Any,
                    5052,
                    options =>
                    {
                        options.Protocols = HttpProtocols.Http1;
                    }
                );
                options.Listen(
                    IPAddress.Any,
                    5053,
                    options =>
                    {
                        options.Protocols = HttpProtocols.Http2;
                    }
                );
            });

            return builder;
        }

        public static void UseGrpcService(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<FullProjectInfoService>();
            });
        }
    }
}
