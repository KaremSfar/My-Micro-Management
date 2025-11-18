using MicroManagement.Activity.WebAPI.Events;

namespace MicroManagement.Activity.WebAPI.Services;

public interface IUserActivityService
{
    public Task NotifyUserActivityEventAsync(UserActivityEvent userActivityEvent);
}