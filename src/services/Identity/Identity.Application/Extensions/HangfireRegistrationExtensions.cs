using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Hangfire.MySql;
using System.Transactions;
using Microsoft.AspNetCore.Builder;
using HangfireBasicAuthenticationFilter;
using Shared.SharedModels;

namespace Identity.Application
{
    public static class HangfireRegistrationExtensions
    {
        public static void RegisterHangfire(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Hangfire");
            services.AddHangfire(
                (serivceProvider, config) =>
                {
                    config.UseStorage(
                        new MySqlStorage(
                            connectionString,
                            new MySqlStorageOptions
                            {
                                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                                QueuePollInterval = TimeSpan.FromSeconds(15),
                                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                PrepareSchemaIfNecessary = true,
                                DashboardJobListLimit = 50000,
                                TransactionTimeout = TimeSpan.FromMinutes(1),
                                TablesPrefix = "Hangfire"
                            }
                        )
                    );
                }
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
                .Get<HangfireCredentials>();

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
    }
}
