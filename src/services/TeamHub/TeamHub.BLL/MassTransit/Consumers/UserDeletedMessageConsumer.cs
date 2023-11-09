using MassTransit;
using Shared.SharedModels;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MassTransit.Consumers
{
    public class UserDeletedMessageConsumer : IConsumer<UserDeletedMessage>
    {
        private readonly IUserRepository _userRepository;

        public UserDeletedMessageConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<UserDeletedMessage> context)
        {
            await _userRepository.DeleteByIdAsync(context.Message.Id);
            await _userRepository.SaveAsync();
        }
    }
}
