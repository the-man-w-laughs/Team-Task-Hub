using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.DBContext;

public partial class TeamHubDbContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<TaskModel> Tasks { get; set; }

    public DbSet<TaskHandler> TasksHandlers { get; set; }

    public DbSet<TeamMember> TeamMembers { get; set; }

    public DbSet<User> Users { get; set; }

    public TeamHubDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
