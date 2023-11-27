using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;
using TeamHub.DAL.Constraints;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("holidays");

            entity.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();

            entity.Property(e => e.Date).HasColumnType("timestamp").HasColumnName("date");
            entity
                .Property(e => e.Name)
                .HasMaxLength(HolidayConstraints.maxNameLength)
                .HasColumnName("name");
        }
    }
}
