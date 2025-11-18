using MicroManagement.Activity.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MicroManagement.Activity.WebAPI.Hubs;

[Authorize]
public class UserActivityHub() : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetUserId().ToString());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetUserId().ToString());
        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetUserId()
        => Guid.Parse(Context.User!.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
}
