namespace Identity.Application.Ports.Services
{
    public interface IEmailConfirmationService
    {
        Task ConfirmEmail(string token, string id);
    }
}
