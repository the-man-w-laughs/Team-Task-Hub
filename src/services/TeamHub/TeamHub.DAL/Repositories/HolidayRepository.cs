using Shared.Repository.Sql;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class HolidayRepository : Repository<TeamHubDbContext, Holiday>, IHolidayRepository
    {
        public HolidayRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }
    }
}
