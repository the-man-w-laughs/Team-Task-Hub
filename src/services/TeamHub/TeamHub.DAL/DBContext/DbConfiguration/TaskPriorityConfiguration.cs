using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Constraints;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.DBContext.DbConfiguration;

public class AppUserConfiguration : IEntityTypeConfiguration<TaskPriority>
{
    public void Configure(EntityTypeBuilder<TaskPriority> builder)
    {
        foreach (var value in Enum.GetValues(typeof(TaskPriorityEnum)))
        {
            int index = (int)value;
            string name = value.ToString();
            var priority = new TaskPriority() { Id = index, Name = name };
            builder.HasData(priority);
        }
    }
}
