namespace TeamHub.DAL.Models;

public partial class TeamMember
{
    public int Id { get; set; }

    public int UsersId { get; set; }

    public int ProjectsId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Project Projects { get; set; } = null!;

    public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();

    public virtual ICollection<TasksHandler> TasksHandlers { get; set; } = new List<TasksHandler>();

    public virtual User Users { get; set; } = null!;
}
