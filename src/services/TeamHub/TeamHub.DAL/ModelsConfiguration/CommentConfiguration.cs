﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

            entity.HasIndex(e => e.UsersId, "fk_users_has_tasks_users1_idx");

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
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity
                .HasOne(d => d.Tasks)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.TasksId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_has_tasks_tasks1");

            entity
                .HasOne(d => d.Users)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_has_tasks_users1");
        }
    }
}
