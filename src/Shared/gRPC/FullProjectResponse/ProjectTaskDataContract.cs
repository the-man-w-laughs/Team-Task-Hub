using System.Runtime.Serialization;
using Shared.Enums;

namespace Shared.gRPC.FullProjectResponse;

[DataContract]
public class ProjectTaskDataContract
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int CreatorId { get; set; }

    [DataMember(Order = 3)]
    public TaskPriorityEnum PriorityId { get; set; }

    [DataMember(Order = 4)]
    public string Content { get; set; }

    [DataMember(Order = 5)]
    public DateTime? Deadline { get; set; }

    [DataMember(Order = 6)]
    public bool IsCompleted { get; set; }

    [DataMember(Order = 7)]
    public DateTime CreatedAt { get; set; }

    [DataMember(Order = 8)]
    public List<int> TasksHandlersIds { get; set; }
}
