using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TaskHandlerConfiguration : IEntityTypeConfiguration<TaskHandler>
    {
        public void Configure(EntityTypeBuilder<TaskHandler> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tasks_handlers");

            entity.HasIndex(e => e.TasksId, "fk_team_members_has_tasks_tasks1_idx");

            entity.HasIndex(e => e.TeamMembersId, "fk_team_members_has_tasks_team_members1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TasksId).HasColumnName("tasks_id");
            entity.Property(e => e.TeamMembersId).HasColumnName("team_members_id");

            entity
                .HasOne(d => d.Tasks)
                .WithMany(p => p.TasksHandlers)
                .HasForeignKey(d => d.TasksId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_has_tasks_tasks1");

            entity
                .HasOne(d => d.TeamMembers)
                .WithMany(p => p.TasksHandlers)
                .HasForeignKey(d => d.TeamMembersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_has_tasks_team_members1");
        }
    }
}
