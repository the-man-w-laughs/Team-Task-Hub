using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.DAL.Models;

namespace Identity.Infrastructure.DbContext.DbConfiguration;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        var teamMember1 = new TeamMember
        {
            Id = 1,
            UserId = 1,
            ProjectId = 1
        };

        var teamMember2 = new TeamMember
        {
            Id = 2,
            UserId = 2,
            ProjectId = 1
        };

        builder.HasData(teamMember1, teamMember2);
    }
}
