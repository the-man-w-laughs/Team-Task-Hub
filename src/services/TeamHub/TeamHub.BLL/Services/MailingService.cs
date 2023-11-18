using AutoMapper;

using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.Services
{
    public class MailingService : IMailingService
    {
        private readonly IMapper _mapper;
        private readonly ITaskHandlerRepository _taskHandlerRepository;
        private readonly IUserRepository _userRepository;

        public MailingService(
            IMapper mapper,
            ITaskHandlerRepository taskHandlerRepository,
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _taskHandlerRepository = taskHandlerRepository;
            _userRepository = userRepository;
        }

        public async Task SendPendingTasks()
        {
            var users = _userRepository.GetAllAsync();
        }
    }
}
