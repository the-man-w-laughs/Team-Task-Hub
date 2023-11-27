namespace Identity.Application.Ports.Services
{
    public interface IEmailConfirmationService
    {
        Task ConfirmEmailAsync(string token, string id);
    }
}
