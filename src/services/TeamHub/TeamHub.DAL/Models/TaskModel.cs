using TeamHub.DAL.Constraints;

namespace TeamHub.DAL.Models;

public partial class TaskModel
{
    public int Id { get; set; }

    public int TeamMemberId { get; set; }

    public int ProjectId { get; set; }

    public TaskPriorityEnum PriorityId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? Deadline { get; set; }

    public sbyte IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual TeamMember TeamMember { get; set; } = null!;

    public virtual Project Projects { get; set; } = null!;

    public virtual ICollection<TaskHandler> TasksHandlers { get; set; } = new List<TaskHandler>();
}
