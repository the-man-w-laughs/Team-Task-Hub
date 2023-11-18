using Identity.Domain.Entities;

namespace Identity.Application.Ports.Utils
{
    public interface IConfirmationEmailSender
    {
        public Task SendEmail(AppUser appUser);
    }
}
