using MicroManagement.Activity.WebAPI.Events;
using MicroManagement.Activity.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MicroManagement.Activity.WebAPI.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IHubContext<UserActivityHub> _hubContext;

    public UserActivityService(IHubContext<UserActivityHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyUserActivityEventAsync(UserActivityEvent userActivityEvent)
    {
        await _hubContext.Clients.Group(userActivityEvent.UserId.ToString())
                  .SendAsync("ReceiveEvent", userActivityEvent);
    }
}