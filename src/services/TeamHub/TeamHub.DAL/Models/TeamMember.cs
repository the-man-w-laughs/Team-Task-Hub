namespace TeamHub.DAL.Models;

public partial class TeamMember
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<TaskModel> CreatedTasks { get; set; } = new List<TaskModel>();

    public virtual ICollection<TaskHandler> TasksHandlers { get; set; } = new List<TaskHandler>();

    public virtual User User { get; set; } = null!;
}
