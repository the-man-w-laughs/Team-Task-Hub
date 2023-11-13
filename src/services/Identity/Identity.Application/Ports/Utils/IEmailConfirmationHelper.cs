using Identity.Domain.Entities;

namespace Identity.Application.Ports.Utils
{
    public interface IEmailConfirmationHelper
    {
        public Task<bool> SendEmail(AppUser appUser);
    }
}
