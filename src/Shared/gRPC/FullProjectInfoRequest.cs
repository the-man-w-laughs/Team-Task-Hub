using System.Runtime.Serialization;

namespace Shared.gRPC
{
    [DataContract]
    public class FullProjectInfoRequest
    {
        [DataMember(Order = 1)]
        public int UserId { get; set; }

        [DataMember(Order = 2)]
        public int ProjectId { get; set; }
    }
}
