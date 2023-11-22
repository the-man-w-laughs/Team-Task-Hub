using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int userId, CancellationToken cancellationToken);
    }
}
