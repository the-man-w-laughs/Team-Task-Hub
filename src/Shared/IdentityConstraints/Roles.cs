using Microsoft.AspNetCore.Identity;

namespace Shared.IdentityConstraints
{
    public static class Roles
    {
        public static IdentityRole<int> UserRole = new IdentityRole<int>()
        {
            Id = 1,
            Name = "User",
            NormalizedName = "USER"
        };

        public static IdentityRole<int> AdminRole = new IdentityRole<int>()
        {
            Id = 2,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };
    }
}
