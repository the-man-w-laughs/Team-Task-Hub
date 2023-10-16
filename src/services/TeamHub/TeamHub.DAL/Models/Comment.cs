namespace TeamHub.DAL.Models;

public partial class Comment
{
    public string Id { get; set; } = null!;

    public int UsersId { get; set; }

    public int TasksId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual TaskModel Tasks { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
