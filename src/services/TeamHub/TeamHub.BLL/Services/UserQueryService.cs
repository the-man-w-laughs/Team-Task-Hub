using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetExistingUserAsync(
            int userId,
            CancellationToken cancellationToken
        )
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException($"User with id {userId} not found.");
            }

            return user;
        }
    }
}
