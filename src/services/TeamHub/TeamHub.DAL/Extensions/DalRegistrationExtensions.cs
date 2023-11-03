using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Repositories;

namespace TeamHub.DAL.Extensions
{
    public static class DalRegistrationExtensions
    {
        public static void RegisterDLLDependencies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<TeamHubDbContext>(builder =>
            {
                var connectionString = configuration.GetConnectionString("Default");
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Connection string cannot be empty.");
                builder.UseLazyLoadingProxies();
                builder.UseNpgsql(connectionString);
            });
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITaskModelRepository, TaskModelRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<ITaskHandlerRepository, TaskHandlerRepository>();
        }
    }
}
