using AutoMapper;
using Microsoft.Extensions.Options;
using Shared.SharedModels;
using Shared.SharedModels.Contracts;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TeamMember;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.Services
{
    public class MailingService : IMailingService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ISmtpClientFactory _smtpClientFactory;
        private readonly IDailyMailMessageBuilder _dailyEmailBuilder;
        private readonly EmailCredentials _options;

        public MailingService(
            IMapper mapper,
            IUserRepository userRepository,
            IOptions<EmailCredentials> options,
            ISmtpClientFactory smtpClientFactory,
            IDailyMailMessageBuilder dailyEmailBuilder
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _smtpClientFactory = smtpClientFactory;
            _dailyEmailBuilder = dailyEmailBuilder;
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
                    var teamTeamberResponseDtos = _mapper.Map<List<TeamMemberDto>>(
                        user.TeamMembers
                    );

                    var message = _dailyEmailBuilder.CreateDailyMailMessage(
                        _options.Email,
                        user,
                        teamTeamberResponseDtos
                    );

                    client.Send(message);
                }
            }
        }
    }
}
