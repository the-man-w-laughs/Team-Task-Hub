using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class DbContextExtensions
    {
        public static void InitializeDatabase<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
