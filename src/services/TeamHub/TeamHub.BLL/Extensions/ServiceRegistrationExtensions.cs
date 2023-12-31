﻿using Microsoft.Extensions.DependencyInjection;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Services;

namespace TeamHub.BLL
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IScheduledEmailService, ScheduledEmailService>();
            services.AddScoped<IDailyMailMessageBuilder, DailyMailMessageBuilder>();

            services.AddScoped<IUserQueryService, UserQueryService>();
            services.AddScoped<ITeamMemberQueryService, TeamMemberQueryService>();
            services.AddScoped<ICommentQueryService, CommentQueryService>();
            services.AddScoped<IProjectQueryService, ProjectQueryService>();
            services.AddScoped<ITaskQueryService, TaskQueryService>();
        }
    }
}
