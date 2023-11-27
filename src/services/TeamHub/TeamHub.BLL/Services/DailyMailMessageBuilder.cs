using System.Net.Mail;
using System.Text;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TeamMember;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class DailyMailMessageBuilder : IDailyMailMessageBuilder
    {
        public MailMessage CreateDailyMailMessage(
            string sourceEmail,
            User user,
            List<TeamMemberDto> teamMemberResponseDtos
        )
        {
            var body = ComposeMailBody(user, teamMemberResponseDtos);
            return new MailMessage
            {
                From = new MailAddress(sourceEmail, "TeamTaskHub"),
                To = { new MailAddress(user.Email) },
                Subject = "Daily email.",
                IsBodyHtml = false,
                Body = body
            };
        }

        private string ComposeMailBody(User user, List<TeamMemberDto> teamMemberResponseDtos)
        {
            var body = new StringBuilder();

            body.AppendLine($"Hello {user.Email},");
            body.AppendLine("Here is the summary of your team's tasks:");

            if (teamMemberResponseDtos.Any())
            {
                foreach (var teamMemberResponseDto in teamMemberResponseDtos)
                {
                    body.AppendLine($"- Project: {teamMemberResponseDto.ProjectName}");

                    if (teamMemberResponseDto.Tasks.Any())
                    {
                        body.AppendLine("  Tasks:");

                        foreach (var task in teamMemberResponseDto.Tasks)
                        {
                            body.AppendLine($"    - Task ID: {task.Id}");
                            body.AppendLine($"      Priority: {task.PriorityId}");
                            body.AppendLine($"      Content: {task.Content}");
                            body.AppendLine(
                                $"      Deadline: {task.Deadline?.ToString("yyyy-MM-dd HH:mm:ss") ?? "No deadline"}"
                            );
                            body.AppendLine(
                                $"      Created At: {task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}"
                            );
                        }
                    }
                    else
                    {
                        body.AppendLine("  No tasks assigned.");
                    }

                    body.AppendLine();
                }
            }
            else
            {
                body.AppendLine("- You don't have any projects.");
            }

            body.AppendLine("Thank you!");

            return body.ToString();
        }
    }
}
