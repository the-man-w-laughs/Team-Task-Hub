namespace TeamHub.DAL.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int TasksId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual TaskModel Task { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
