using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Constraints;
using TeamHub.DAL.Models;

namespace Identity.Infrastructure.DbContext.DbConfiguration;

public class TaskHandlerConfiguration : IEntityTypeConfiguration<TaskHandler>
{
    public void Configure(EntityTypeBuilder<TaskHandler> builder)
    {
        var taskHandler1 = new TaskHandler
        {
            Id = 1,
            TasksId = 1,
            TeamMemberId = 2
        };

        builder.HasData(taskHandler1);
    }
}
