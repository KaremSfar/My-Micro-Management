using LanguageExt.Pipes;
using MicroManagement.Service.WebAPI.Services;
using MicroManagement.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MicroManagement.Service.WebAPI.Hubs;

[Authorize]
public class TimeSessionsHub(IUserConnectionsProvider _userConnectionsProvider, ITimeSessionsService _timeSessionsService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        await _userConnectionsProvider.AddConnection(GetUserId(), Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetUserId().ToString());

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _userConnectionsProvider.RemoveConnection(GetUserId(), Context.ConnectionId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetUserId().ToString());

        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetUserId()
           => Guid.Parse(Context.User!.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
}
