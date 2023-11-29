using AutoMapper;
using MassTransit;
using Shared.SharedModels;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MassTransit.Consumers
{
    public class UserCreatedMessageConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserCreatedMessageConsumer(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            var userToCreate = _mapper.Map<User>(context.Message);
            await _userRepository.AddAsync(userToCreate);
            await _userRepository.SaveAsync();
        }
    }
}
