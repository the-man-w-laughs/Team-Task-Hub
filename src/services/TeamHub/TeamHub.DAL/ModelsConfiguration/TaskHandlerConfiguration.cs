using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TaskHandlerConfiguration : IEntityTypeConfiguration<TaskHandler>
    {
        public void Configure(EntityTypeBuilder<TaskHandler> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("tasks_handlers");

            entity.HasIndex(e => e.TaskId, "fk_team_members_has_tasks_tasks1_idx");

            entity.HasIndex(e => e.TeamMemberId, "fk_team_members_has_tasks_team_members1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TaskId).HasColumnName("tasks_id");
            entity.Property(e => e.TeamMemberId).HasColumnName("team_members_id");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");

            entity
                .HasOne(d => d.Task)
                .WithMany(p => p.TasksHandlers)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_has_tasks_tasks1");

            entity
                .HasOne(d => d.TeamMember)
                .WithMany(p => p.TasksHandlers)
                .HasForeignKey(d => d.TeamMemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_has_tasks_team_members1");
        }
    }
}
