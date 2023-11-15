using System.Runtime.Serialization;

namespace Shared.gRPC.FullProjectResponse;

[DataContract]
public partial class UserDataContract
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Email { get; set; }

    [DataMember(Order = 3)]
    public DateTime CreatedAt { get; set; }
}
