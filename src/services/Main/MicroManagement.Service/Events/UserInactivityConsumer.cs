using MassTransit;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction;
using MicroManagement.Shared.Events;

namespace MicroManagement.Service.WebAPI.Events
{
    public class UserInactivityConsumer : IConsumer<UserInactiveEvent>
    {
        private readonly ITimeSessionsService _timeSessionsService;

        public UserInactivityConsumer(ITimeSessionsService timeSessionsService)
        {
            _timeSessionsService = timeSessionsService;
        }

        public async Task Consume(ConsumeContext<UserInactiveEvent> context)
        {
            var message = context.Message;
            if (message == null || message.UserId == Guid.Empty)
            {
                return; // Ignore invalid messages
            }

            await _timeSessionsService.StopTimeSession(message.UserId);
        }
    }
}
