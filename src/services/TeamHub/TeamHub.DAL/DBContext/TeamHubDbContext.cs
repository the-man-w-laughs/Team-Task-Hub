using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.DBContext;

public partial class TeamHubDbContext : DbContext
{
    public TeamHubDbContext() { }

    public TeamHubDbContext(DbContextOptions<TeamHubDbContext> options)
        : base(options) { }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<TaskModel> Tasks { get; set; }

    public virtual DbSet<TaskHandler> TasksHandlers { get; set; }

    public virtual DbSet<TeamMember> TeamMembers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
