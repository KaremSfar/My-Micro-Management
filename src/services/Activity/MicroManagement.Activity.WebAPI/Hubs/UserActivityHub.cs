using MicroManagement.Activity.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MicroManagement.Activity.WebAPI.Hubs;

[Authorize]
public class UserActivityHub(IUserActivityManager _userActivityManager) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        
        // Register for events on connection
        await RegisterForEventsAsync();
        
        await base.OnConnectedAsync();
    }

    private async Task RegisterForEventsAsync()
    {
        var userId = GetUserId();
        var cancellationToken = Context.ConnectionAborted;

        try
        {
            var events = await _userActivityManager.GetEvents(userId, cancellationToken);

            // Stream events asynchronously to the connected client
            await foreach (var userActivityEvent in events.WithCancellation(cancellationToken))
            {
                await Clients.Caller.SendAsync("ReceiveEvent", userActivityEvent, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Connection was closed, exit gracefully
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetUserId().ToString());
        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetUserId()
        => Guid.Parse(Context.User!.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
}
