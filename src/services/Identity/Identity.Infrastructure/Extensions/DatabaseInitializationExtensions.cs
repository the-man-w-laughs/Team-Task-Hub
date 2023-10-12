using Identity.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
