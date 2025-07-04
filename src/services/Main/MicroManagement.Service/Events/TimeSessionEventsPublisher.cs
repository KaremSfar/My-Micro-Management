using MassTransit;
using MicroManagement.Shared.Events;

namespace MicroManagement.Service.WebAPI.Events;

public class TimeSessionEventsPublisher : ITimeSessionEventsPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TimeSessionEventsPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishTimeSessionStartedEventAsync(Guid userId, Guid projectId)
    {
        var timeSessionStartedEvent = new TimeSessionStartedEvent
        {
            UserId = userId,
            ProjectId = projectId
        };

        await _publishEndpoint.Publish(timeSessionStartedEvent);
    }

    public Task PublishTimeSessionStoppedEventAsync(Guid userId)
    {
        var timeSessionStoppedEvent = new TimeSessionStoppedEvent
        {
            UserId = userId
        };

        return _publishEndpoint.Publish(timeSessionStoppedEvent);
    }
}
