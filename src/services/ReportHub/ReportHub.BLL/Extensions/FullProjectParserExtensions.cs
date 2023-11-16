using System.Text;
using TeamHub.BLL.Dtos;

namespace ReportHub.BLL.Extensions
{
    public static class FullProjectParserExtensions
    {
        public static string ToReport(this FullProjectResponseDto project)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Report Creation Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n");
            sb.AppendLine($"Project ID: {project.Id}");
            sb.AppendLine($"Name: {project.Name}");
            sb.AppendLine($"Created At: {project.CreatedAt}");
            sb.AppendLine($"Creator: {project.Creator.Email}");
            sb.AppendLine("Team Members:");

            foreach (var teamMember in project.TeamMembers)
            {
                sb.AppendLine($"\t- {teamMember.Email}");
            }

            sb.AppendLine("Tasks:");

            foreach (var task in project.Tasks)
            {
                sb.AppendLine($"\tTask ID: {task.Id}");
                sb.AppendLine($"\tPriority: {task.PriorityId.ToString()}");
                sb.AppendLine($"\tContent: {task.Content}");
                sb.AppendLine($"\tCreated At: {task.CreatedAt}");
                sb.AppendLine($"\tIs Completed: {task.IsCompleted}");
                sb.AppendLine("\tTask Handlers:");

                foreach (var handlerId in task.TasksHandlersIds)
                {
                    var handler = project.TeamMembers.FirstOrDefault(u => u.Id == handlerId);

                    if (handler != null)
                    {
                        sb.AppendLine($"\t\t- User: {handler.Email}");
                    }
                    else
                    {
                        sb.AppendLine($"\t\t- User ID: {handlerId}");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
