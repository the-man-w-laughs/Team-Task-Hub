using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;
using TeamHub.DAL.Constraints;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TaskModelConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("tasks");

            entity.HasIndex(e => e.ProjectId, "fk_tasks_projects1_idx");

            entity.HasIndex(e => e.TeamMemberId, "fk_tasks_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity
                .Property(e => e.Content)
                .HasMaxLength(TaskModelConstraints.maxContentLength)
                .HasColumnName("content");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.TeamMemberId).HasColumnName("creator_id");
            entity.Property(e => e.Deadline).HasColumnType("timestamp").HasColumnName("deadline");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.PriorityId).HasColumnName("priority_id");
            entity.Property(e => e.ProjectId).HasColumnName("projects_id");

            entity
                .HasOne(d => d.TeamMember)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TeamMemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tasks_users1");

            entity
                .HasOne(d => d.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tasks_projects1");
        }
    }
}
