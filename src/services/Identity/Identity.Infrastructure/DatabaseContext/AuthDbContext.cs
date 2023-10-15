using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Identity.Application.Ports.Repositories;

namespace Identity.Infrastructure.DatabaseContext
{
    public class AuthDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>, IAuthDBContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetRoles(builder);
            SetUsers(builder);
            SetUserRoles(builder);
        }

        private static void SetRoles(ModelBuilder builder)
        {
            builder
                .Entity<IdentityRole<int>>()
                .HasData(
                    new List<IdentityRole<int>>()
                    {
                        Domain.Constraints.Roles.UserRole,
                        Domain.Constraints.Roles.AdminRole
                    }
                );
        }

        private static void SetUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<AppUser>();
            var admin = new AppUser()
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "admin")
            };
            builder.Entity<AppUser>().HasData(admin);
        }

        private static void SetUserRoles(ModelBuilder builder)
        {
            builder
                .Entity<IdentityUserRole<int>>()
                .HasData(new IdentityUserRole<int>() { RoleId = 2, UserId = 1 });
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
