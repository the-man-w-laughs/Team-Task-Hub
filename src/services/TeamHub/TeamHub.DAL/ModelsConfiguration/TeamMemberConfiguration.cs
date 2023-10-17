using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Models;

namespace TeamHub.DAL.ModelsConfiguration
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("team_members");

            entity.HasIndex(e => e.ProjectsId, "fk_team_members_projects1_idx");

            entity.HasIndex(e => e.UsersId, "fk_team_members_users_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at");
            entity.Property(e => e.ProjectsId).HasColumnName("projects_id");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity
                .HasOne(d => d.Projects)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.ProjectsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_projects1");

            entity
                .HasOne(d => d.Users)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_members_users");
        }
    }
}
