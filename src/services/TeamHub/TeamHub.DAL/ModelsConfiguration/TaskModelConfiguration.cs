using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TaskModelConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tasks");

            entity.HasIndex(e => e.PriorityId, "fk_tasks_priority1_idx");

            entity.HasIndex(e => e.ProjectsId, "fk_tasks_projects1_idx");

            entity.HasIndex(e => e.CreatorId, "fk_tasks_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasMaxLength(256).HasColumnName("content");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.Deadline).HasColumnType("datetime").HasColumnName("deadline");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.PriorityId).HasColumnName("priority_id");
            entity.Property(e => e.ProjectsId).HasColumnName("projects_id");

            entity
                .HasOne(d => d.Creator)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("fk_tasks_users1");

            entity
                .HasOne(d => d.Priority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tasks_priority1");

            entity
                .HasOne(d => d.Projects)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectsId)
                .HasConstraintName("fk_tasks_projects1");
        }
    }
}
