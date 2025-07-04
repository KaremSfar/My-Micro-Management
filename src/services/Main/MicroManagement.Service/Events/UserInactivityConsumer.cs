using MassTransit;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction;
using MicroManagement.Shared.Events;

namespace MicroManagement.Service.WebAPI.Events
{
    public class UserInactivityConsumer : IConsumer<UserInactiveEvent>
    {
        private readonly ITimeSessionsService _timeSessionsService;
        private static readonly HttpClient _httpClient = new HttpClient();

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

            // Notify phone of inactivity
            await _httpClient.GetAsync("https://ksgn8nvps.duckdns.org/webhook/72f643ab-7c58-4ff3-8c3e-9a6855fa9a38");
        }
    }
}
