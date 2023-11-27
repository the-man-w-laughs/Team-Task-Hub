using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.DBContext.DbConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var admin = new User
        {
            Id = 1,
            Email = "admin@the.best",
            CreatedAt = DateTime.Now
        };
        builder.HasData(admin);
    }
}
