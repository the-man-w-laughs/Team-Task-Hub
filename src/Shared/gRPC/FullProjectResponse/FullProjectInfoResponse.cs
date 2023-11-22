using System.Runtime.Serialization;

namespace Shared.gRPC.FullProjectResponse;

[DataContract]
public class FullProjectInfoResponse
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int AuthorId { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    public DateTime CreatedAt { get; set; }

    [DataMember(Order = 5)]
    public UserDataContract Creator { get; set; }

    [DataMember(Order = 6)]
    public List<UserDataContract> TeamMembers { get; set; }

    [DataMember(Order = 7)]
    public List<ProjectTaskDataContract> Tasks { get; set; }
}
