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

        public static void UseGrpcService(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<FullProjectInfoService>();
            });
        }
    }
}
