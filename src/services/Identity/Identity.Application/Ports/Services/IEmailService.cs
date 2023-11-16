namespace Identity.Application.Ports.Services
{
    public interface IEmailService
    {
        Task ConfirmEmail(string token, string id);
    }
}
