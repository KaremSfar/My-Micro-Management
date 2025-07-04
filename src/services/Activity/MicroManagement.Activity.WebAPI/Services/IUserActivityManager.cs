using MicroManagement.Activity.WebAPI.Events;

namespace MicroManagement.Activity.WebAPI.Services
{
    public interface IUserActivityManager
    {
        Task<IAsyncEnumerable<UserActivityEvent>> GetEvents(Guid userId, CancellationToken cancellationToken = default);
        Task RegisterEvent(UserActivityEvent e);
    }
}