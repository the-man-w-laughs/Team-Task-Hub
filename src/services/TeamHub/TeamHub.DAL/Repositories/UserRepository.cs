using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }
    }
}
