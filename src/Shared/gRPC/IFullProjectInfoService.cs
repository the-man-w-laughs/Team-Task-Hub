using System.ServiceModel;
using Shared.gRPC.FullProjectResponse;

namespace Shared.gRPC;

[ServiceContract]
public interface IFullProjectInfoService
{
    [OperationContract]
    Task<FullProjectInfoResponse> GetFullProjectInfoAsync(
        FullProjectInfoRequest fullProjectInfoRequest
    );
}
