using Identity.Domain.Constraints;
using Identity.Domain.Entities;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.WebAPI.Extensions.Identity
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = roles.Select(role => new Claim(ClaimNames.RoleClaimName, role));

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);                        

            context.IsActive = (user != null);
        }
    }
}
