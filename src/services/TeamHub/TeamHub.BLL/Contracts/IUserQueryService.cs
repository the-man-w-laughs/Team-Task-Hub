using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface IUserQueryService
    {
        Task<User> GetExistingUserAsync(int userId, CancellationToken cancellationToken);
    }
}
