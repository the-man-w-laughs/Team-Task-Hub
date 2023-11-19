using System.Net.Mail;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Shared.SharedModels;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TeamMember;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class MailingService : IMailingService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly EmailCredentials _options;

        public MailingService(
            IMapper mapper,
            IUserRepository userRepository,
            IOptions<EmailCredentials> options
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _options = options.Value;
        }

        public async Task SendPendingTasks()
        {
            var users = await _userRepository.GetAllAsync();

            using (var client = new SmtpClient(host: _options.Host, port: 587))
            {
                client.Credentials = new System.Net.NetworkCredential(
                    _options.Email,
                    _options.AppPassword
                );
                client.EnableSsl = true;

                foreach (var user in users)
                {
                    var teamTeamberResponseDtos = _mapper.Map<List<TeamMemberResponseDto>>(
                        user.TeamMembers
                    );

                    var body = ComposeMailBody(user, teamTeamberResponseDtos);
                    var massage = CreateMailMessage(user.Email, body);

                    try
                    {
                        client.Send(massage);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private MailMessage CreateMailMessage(string toEmail, string body)
        {
            return new MailMessage
            {
                From = new MailAddress(_options.Email, "TeamTaskHub"),
                To = { new MailAddress(toEmail) },
                Subject = "Daily email.",
                IsBodyHtml = false,
                Body = body
            };
        }

        private string ComposeMailBody(
            User user,
            List<TeamMemberResponseDto> teamMemberResponseDtos
        )
        {
            var body = new StringBuilder();

            body.AppendLine($"Hello {user.Email},");
            body.AppendLine("Here is the summary of your team's tasks:");

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

            body.AppendLine("Thank you!");

            return body.ToString();
        }
    }
}
