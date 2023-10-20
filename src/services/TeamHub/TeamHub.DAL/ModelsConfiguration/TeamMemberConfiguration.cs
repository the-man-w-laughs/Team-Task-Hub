using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("team_members");

            entity.HasIndex(e => e.ProjectId, "fk_team_members_projects1_idx");

            entity.HasIndex(e => e.UserId, "fk_team_members_users_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.ProjectId).HasColumnName("projects_id");
            entity.Property(e => e.UserId).HasColumnName("users_id");

            entity
                .HasOne(d => d.Projects)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_projects1");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_users");
        }
    }
}
