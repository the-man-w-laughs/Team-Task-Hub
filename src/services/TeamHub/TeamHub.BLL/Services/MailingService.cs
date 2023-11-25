using System.Net.Mail;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Shared.SharedModels;
using Shared.SharedModels.Contracts;
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
        private readonly ISmtpClientFactory _smtpClientFactory;
        private readonly EmailCredentials _options;

        public MailingService(
            IMapper mapper,
            IUserRepository userRepository,
            IOptions<EmailCredentials> options,
            ISmtpClientFactory smtpClientFactory
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _smtpClientFactory = smtpClientFactory;
            _options = options.Value;
        }

        public async Task SendPendingTasksAsync()
        {
            var users = await _userRepository.GetAllAsync();

            using (
                var client = _smtpClientFactory.CreateSmtpClient(
                    _options.Host,
                    _options.Email,
                    _options.AppPassword
                )
            )
            {
                foreach (var user in users)
                {
                    var teamTeamberResponseDtos = _mapper.Map<List<TeamMemberResponseDto>>(
                        user.TeamMembers
                    );

                    var body = ComposeMailBody(user, teamTeamberResponseDtos);
                    var massage = CreateMailMessage(user.Email, body);

                    client.Send(massage);
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
