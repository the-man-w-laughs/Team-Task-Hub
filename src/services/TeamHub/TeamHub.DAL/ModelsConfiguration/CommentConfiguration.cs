using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;
using TeamHub.DAL.Constraints;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("comments");

            entity.HasIndex(e => e.TasksId, "fk_users_has_tasks_tasks1_idx");

            entity.HasIndex(e => e.AuthorId, "fk_users_has_tasks_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity
                .Property(e => e.Content)
                .HasMaxLength(CommentConstraints.maxContentLength)
                .HasColumnName("content");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.TasksId).HasColumnName("tasks_id");
            entity.Property(e => e.AuthorId).HasColumnName("users_id");

            entity
                .HasOne(d => d.Task)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.TasksId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_has_tasks_tasks1");

            entity
                .HasOne(d => d.Author)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_has_tasks_users1");
        }
    }
}
