using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Models;

namespace Identity.Infrastructure.DbContext.DbConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var admin = new User
        {
            Id = 1,
            Email = "admin@cool.best",
            CreatedAt = DateTime.Now
        };

        var user1 = new User
        {
            Id = 2,
            Email = "the@weakest.potato",
            CreatedAt = DateTime.Now
        };

        builder.HasData(admin, user1);
    }
}
