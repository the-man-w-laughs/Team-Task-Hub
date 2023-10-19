namespace TeamHub.DAL.Models;

public partial class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
