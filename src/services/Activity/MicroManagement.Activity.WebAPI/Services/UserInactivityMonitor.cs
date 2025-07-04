using Coravel.Invocable;
using MassTransit;
using MicroManagement.Shared.Events;

namespace MicroManagement.Activity.WebAPI.Services
{
    public class UserInactivityMonitor : IInvocable
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserInactivityMonitor(IUserConnectionRepository userConnectionRepository, IPublishEndpoint publishEndpoint)
        {
            _userConnectionRepository = userConnectionRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Invoke()
        {
            var inactiveUsers = await _userConnectionRepository.GetAllInactiveUsersAsync();

            var now = DateTime.UtcNow;

            var inactiveThreshold = now.AddSeconds(-5);

            foreach (var user in inactiveUsers)
            {
                if ((DateTime.UtcNow - user.UpdatedAt.Value) > TimeSpan.FromMinutes(5)
                    && (DateTime.UtcNow - user.UpdatedAt.Value) < TimeSpan.FromMinutes(10))
                {
                    await _publishEndpoint.Publish(new UserInactiveEvent
                    {
                        UserId = user.UserId,
                        InactiveSince = user.UpdatedAt.Value
                    });
                }
            }
        }
    }
}
