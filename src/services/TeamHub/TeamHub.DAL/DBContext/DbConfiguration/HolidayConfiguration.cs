using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.DBContext.DbConfiguration;

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        var holidays = new List<Holiday>
        {
            new Holiday
            {
                Id = 1,
                Name = "New Year's Day",
                Date = new DateTime(1, 1, 1)
            },
            new Holiday
            {
                Id = 2,
                Name = "New Year (non-working day in Belarus)",
                Date = new DateTime(1, 1, 2)
            },
            new Holiday
            {
                Id = 3,
                Name = "Easter Monday",
                Date = new DateTime(1, 4, 1)
            },
            new Holiday
            {
                Id = 4,
                Name = "Labour Day",
                Date = new DateTime(1, 5, 1)
            },
            new Holiday
            {
                Id = 5,
                Name = "Victory Day",
                Date = new DateTime(1, 5, 8)
            },
            new Holiday
            {
                Id = 6,
                Name = "Victory Day (Soviet)",
                Date = new DateTime(1, 5, 9)
            },
            new Holiday
            {
                Id = 7,
                Name = "Independence Day (Belarus)",
                Date = new DateTime(1, 7, 3)
            },
            new Holiday
            {
                Id = 8,
                Name = "Assumption Day",
                Date = new DateTime(1, 8, 15)
            },
            new Holiday
            {
                Id = 9,
                Name = "All Saints' Day",
                Date = new DateTime(1, 11, 1)
            },
            new Holiday
            {
                Id = 10,
                Name = "Armistice Day",
                Date = new DateTime(1, 11, 11)
            },
            new Holiday
            {
                Id = 11,
                Name = "Christmas Day",
                Date = new DateTime(1, 12, 25)
            }
        };

        builder.HasData(holidays);
    }
}
