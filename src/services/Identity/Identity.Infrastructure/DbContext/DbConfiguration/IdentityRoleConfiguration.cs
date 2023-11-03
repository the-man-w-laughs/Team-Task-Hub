using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.IdentityConstraints;

namespace Identity.Infrastructure.DbConfiguration;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        builder.HasData(new List<IdentityRole<int>>() { Roles.UserRole, Roles.AdminRole });
    }
}
