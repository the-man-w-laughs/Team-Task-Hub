using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Constraints;
using TeamHub.DAL.Models;

namespace Identity.Infrastructure.DbContext.DbConfiguration;

public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
{
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        var task1 = new TaskModel
        {
            Id = 1,
            TeamMemberId = 1,
            ProjectId = 1,
            PriorityId = TaskPriorityEnum.High,
            Content = "Simple Task 1",
            CreatedAt = DateTime.Now
        };

        builder.HasData(task1);
    }
}
