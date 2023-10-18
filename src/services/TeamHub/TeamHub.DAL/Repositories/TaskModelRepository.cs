using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.Repositories
{
    public class TaskModelRepository : Repository<TaskModel>, ITaskModelRepository
    {
        public TaskModelRepository(TeamHubDbContext TeamHubDbContext)
            : base(TeamHubDbContext) { }
    }
}
