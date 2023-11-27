using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.DBContext.DbConfiguration;

public class TaskHandlerConfiguration : IEntityTypeConfiguration<TaskHandler>
{
    public void Configure(EntityTypeBuilder<TaskHandler> builder) { }
}
