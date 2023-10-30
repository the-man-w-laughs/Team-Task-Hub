using System.ServiceModel;
using Shared.gRPC.FullProjectResponse;

namespace Shared.gRPC;

[ServiceContract]
public interface IFullProjectInfoService
{
    [OperationContract]
    Task<FullProjectInfoResponse> GetProjectTaskAsync(
        FullProjectInfoRequest fullProjectInfoRequest
    );
}
