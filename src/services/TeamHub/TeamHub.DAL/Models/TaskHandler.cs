namespace TeamHub.DAL.Models;

public partial class TaskHandler
{
    public int Id { get; set; }
    public int TeamMemberId { get; set; }
    public int TaskId { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual TaskModel Task { get; set; } = null!;
    public virtual TeamMember TeamMember { get; set; } = null!;
}
