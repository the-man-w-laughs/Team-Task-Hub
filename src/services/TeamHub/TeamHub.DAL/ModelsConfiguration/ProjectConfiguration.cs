﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;
using TeamHub.DAL.Constraints;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("projects");

            entity.HasIndex(e => e.AuthorId, "fk_projects_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.AuthorId).HasColumnName("creator_id");
            entity
                .Property(e => e.Name)
                .HasMaxLength(ProjectConstraints.maxNameLength)
                .HasColumnName("name");

            entity
                .HasOne(d => d.Creator)
                .WithMany(p => p.CreatedProjects)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_projects_users1");
        }
    }
}
