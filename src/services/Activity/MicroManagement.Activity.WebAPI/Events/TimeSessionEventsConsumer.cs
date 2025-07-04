using MassTransit;
using MicroManagement.Activity.WebAPI.Services;
using MicroManagement.Shared.Events;
using System.Text.Json;

namespace MicroManagement.Activity.WebAPI.Events;

public class TimeSessionEventsConsumer : IConsumer<TimeSessionStartedEvent>, IConsumer<TimeSessionStoppedEvent>
{
    private readonly ILogger<TimeSessionEventsConsumer> _logger;
    private readonly UserActivityManager _userActivityManager;

    public TimeSessionEventsConsumer(
        ILogger<TimeSessionEventsConsumer> logger,
        UserActivityManager userActivityManager)
    {
        _logger = logger;
        _userActivityManager = userActivityManager;
    }

    public async Task Consume(ConsumeContext<TimeSessionStartedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Time session started for User: {UserId}, Project: {ProjectId}",
            message.UserId, message.ProjectId);

        // Broadcast the event to all connected clients
        await _userActivityManager.RegisterEvent(new UserActivityEvent
        {
            UserId = message.UserId,
            UserActivityEventType = UserActivityEventType.TimeSessionStarted,
            EventData = message
        });
    }

    public Task Consume(ConsumeContext<TimeSessionStoppedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Time session stopped for User: {UserId}", message.UserId);

        // Broadcast the event to all connected clients
        return _userActivityManager.RegisterEvent(
            new UserActivityEvent
            {
                UserId = message.UserId,
                UserActivityEventType = UserActivityEventType.TimeSessionStopped,
                EventData = message
            });
    }
}
