using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity
                .Property(e => e.LastActiveAt)
                .HasColumnType("datetime")
                .HasColumnName("last_active_at");
            entity.Property(e => e.PasswordHash).HasMaxLength(32).HasColumnName("password_hash");
            entity.Property(e => e.Salt).HasMaxLength(32).HasColumnName("salt");
        }
    }
}
