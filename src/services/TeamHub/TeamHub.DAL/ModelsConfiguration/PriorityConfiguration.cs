using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;
using TeamHub.DAL.Constraints;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TaskPriorityConfiguration : IEntityTypeConfiguration<TaskPriority>
    {
        public void Configure(EntityTypeBuilder<TaskPriority> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("priority");

            entity.Property(e => e.Id).HasColumnName("id");
            entity
                .Property(e => e.Name)
                .HasMaxLength(PriorityConstraints.maxNameLength)
                .HasColumnName("name");
        }
    }
}
