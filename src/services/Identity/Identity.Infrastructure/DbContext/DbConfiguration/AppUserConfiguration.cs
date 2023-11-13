using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.DbContext.DbConfiguration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        var hasher = new PasswordHasher<AppUser>();
        var username = "admin";
        var email = "admin@the.best";

        var admin = new AppUser()
        {
            Id = 1,
            UserName = username,
            Email = email,
            NormalizedUserName = "ADMIN",
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = hasher.HashPassword(null, "admin")
        };

        builder.HasData(admin);
    }
}
