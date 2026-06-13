using CoreApplication.API.Hubs;
using CoreApplication.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CoreApplication.API.Services;

public class SignalRLocationBroadcaster(IHubContext<LocationHub> hubContext) : ILiveLocationBroadcaster
{
    public async Task BroadcastAsync(LiveLocationDto dto, CancellationToken ct = default)
    {
        await hubContext.Clients.Group("admin-tracking")
            .SendAsync("ReceiveLocation", dto, ct);

        await hubContext.Clients.Group($"courier-{dto.CourierId}")
            .SendAsync("ReceiveLocation", dto, ct);
    }
}
