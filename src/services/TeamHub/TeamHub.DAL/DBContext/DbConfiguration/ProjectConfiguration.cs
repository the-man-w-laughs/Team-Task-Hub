using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Constraints;
using TeamHub.DAL.Models;

namespace Identity.Infrastructure.DbContext.DbConfiguration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        var project1 = new Project
        {
            Id = 1,
            AuthorId = 1,
            Name = "High priority project",
            CreatedAt = DateTime.Now
        };

        builder.HasData(project1);
    }
}
