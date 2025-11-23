using MassTransit;
using MicroManagement.Activity.WebAPI.Services;
using MicroManagement.Shared.Events;
using System.Text.Json;

namespace MicroManagement.Activity.WebAPI.Events;

public class TimeSessionEventsConsumer : IConsumer<TimeSessionStartedEvent>, IConsumer<TimeSessionStoppedEvent>
{
    private readonly ILogger<TimeSessionEventsConsumer> _logger;
    private readonly IUserActivityService _userActivityService;

    public TimeSessionEventsConsumer(
        ILogger<TimeSessionEventsConsumer> logger, IUserActivityService userActivityService)
    {
        _logger = logger;
        _userActivityService = userActivityService;
    }

    public async Task Consume(ConsumeContext<TimeSessionStartedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Time session started for User: {UserId}, Project: {ProjectId}",
            message.UserId, message.ProjectId);

        await this._userActivityService.NotifyUserActivityEventAsync(new UserActivityEvent
        {
            UserId = message.UserId,
            UserActivityEventType = UserActivityEventType.TimeSessionStarted,
            EventData = message
        });
    }

    public async Task Consume(ConsumeContext<TimeSessionStoppedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Time session stopped for User: {UserId}", message.UserId);

       await this._userActivityService.NotifyUserActivityEventAsync(new UserActivityEvent
        {
            UserId = message.UserId,
            UserActivityEventType = UserActivityEventType.TimeSessionStopped,
            EventData = message
        });
    }
}
