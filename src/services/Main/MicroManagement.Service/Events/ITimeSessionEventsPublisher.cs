
namespace MicroManagement.Service.WebAPI.Events
{
    public interface ITimeSessionEventsPublisher
    {
        Task PublishTimeSessionStartedEventAsync(Guid userId, Guid projectId);
        Task PublishTimeSessionStoppedEventAsync(Guid userId);
    }
}