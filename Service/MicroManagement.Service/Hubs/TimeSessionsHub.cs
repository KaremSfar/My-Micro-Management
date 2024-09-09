using LanguageExt.Pipes;
using MicroManagement.Core;
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

    public async Task TimeSessionStarted(Guid projectId)
    {
        var userId = GetUserId();

        await _timeSessionsService.StopTimeSession(userId);

        await Clients.GroupExcept(userId.ToString(), [Context.ConnectionId]).SendAsync("TimeSessionsStopped");

        await _timeSessionsService.StartTimeSession(userId, projectId);

        await Clients.GroupExcept(userId.ToString(), [Context.ConnectionId]).SendAsync("TimeSessionStarted", projectId);
    }

    public async Task TimeSessionsStopped()
    {
        var userId = GetUserId();

        await _timeSessionsService.StopTimeSession(userId);

        await Clients.GroupExcept(userId.ToString(), [Context.ConnectionId]).SendAsync("TimeSessionsStopped");
    }

    private Guid GetUserId()
           => Guid.Parse(Context.User!.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
}
