using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Shared.SharedModels;
using HangfireBasicAuthenticationFilter;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL
{
    public static class HangfireRegistrationExtensions
    {
        public static void RegisterHangfire(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Hangfire");
            services.AddHangfire(
                config => config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString))
            );
            services.AddHangfireServer();
        }

        public static IApplicationBuilder UseHangfireWithDashboard(
            this IApplicationBuilder app,
            IConfiguration configuration
        )
        {
            var hangfireCredentials = configuration
                .GetSection("HangfireCredentials")
                .Get<HangfireCredentials>()!;

            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions
                {
                    DashboardTitle = "Email confirmation dashboard",
                    Authorization = new[]
                    {
                        new HangfireCustomBasicAuthenticationFilter
                        {
                            User = hangfireCredentials.User,
                            Pass = hangfireCredentials.Password
                        }
                    }
                }
            );

            return app;
        }

        public static IApplicationBuilder UseDailyMailing(this IApplicationBuilder app)
        {
            string recurringJobId = "DailyMailingJob";

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
            var recurringJobOptions = new RecurringJobOptions { TimeZone = timeZone };

            RecurringJob.AddOrUpdate<IScheduledEmailService>(
                recurringJobId,
                scheduledEmailService => scheduledEmailService.Schedule(),
                Cron.Daily(8), // Adjusted to run at 8 AM
                recurringJobOptions
            );

            return app;
        }
    }
}
