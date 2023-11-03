using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace ReportHub.BLL.Extensions.Utils
{
    public class GrpcClientFactory
    {
        private readonly GrpcChannel _channel;

        public GrpcClientFactory(string address)
        {
            _channel = GrpcChannel.ForAddress(address);
        }

        public TClient CreateClient<TClient>()
            where TClient : class
        {
            return _channel.CreateGrpcService<TClient>();
        }
    }
}
