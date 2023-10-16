namespace TeamHub.DAL.Models;

public partial class TaskPriority
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}
