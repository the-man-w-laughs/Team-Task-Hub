using System.Text;
using Shared.gRPC.FullProjectResponse;

namespace ReportHub.BLL.Extensions
{
    public static class FullProjectParserExtensions
    {
        public static string ToReport(this FullProjectInfoResponse fullProjectInfo)
        {
            var report = new StringBuilder();
            report.AppendLine($"Report Creation Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n");
            report.AppendLine($"Project ID: {fullProjectInfo.Id}");
            report.AppendLine($"Name: {fullProjectInfo.Name}");
            report.AppendLine($"Created At: {fullProjectInfo.CreatedAt}");
            report.AppendLine($"Creator: {fullProjectInfo.Creator.Email}");

            report.AppendLine("Team Members:");
            if (fullProjectInfo.TeamMembers != null)
            {
                foreach (var teamMember in fullProjectInfo.TeamMembers)
                {
                    report.AppendLine($"\t- {teamMember.Email}");
                }
            }
            else
            {
                report.AppendLine($"\tNo Team Members");
            }

            report.AppendLine("Tasks:");
            if (fullProjectInfo.Tasks != null)
            {
                foreach (var task in fullProjectInfo.Tasks)
                {
                    report.AppendLine($"\tTask ID: {task.Id}");
                    report.AppendLine($"\tPriority: {task.PriorityId.ToString()}");
                    report.AppendLine($"\tContent: {task.Content}");
                    report.AppendLine($"\tCreated At: {task.CreatedAt}");
                    report.AppendLine($"\tIs Completed: {task.IsCompleted}");

                    report.AppendLine("\tTask Handlers:");

                    if (task.TasksHandlersIds != null)
                    {
                        foreach (var handlerId in task.TasksHandlersIds)
                        {
                            var handler = fullProjectInfo.TeamMembers.FirstOrDefault(
                                u => u.Id == handlerId
                            );

                            if (handler != null)
                            {
                                report.AppendLine($"\t\t- User: {handler.Email}");
                            }
                            else
                            {
                                report.AppendLine($"\t\t- User ID: {handlerId}");
                            }
                        }
                    }
                    else
                    {
                        report.AppendLine($"\t\tNo Task Handlers");
                    }
                }
            }
            else
            {
                report.AppendLine($"\tNo tasks");
            }

            return report.ToString();
        }
    }
}
