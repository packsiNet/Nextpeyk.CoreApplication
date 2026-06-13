using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CoreApplication.API.Hubs;

[Authorize(Roles = "Admin,CourierManager,CourierOperator")]
public class LocationHub : Hub
{
    /// <summary>Admin subscribes to ALL couriers' drivers.</summary>
    [Authorize(Roles = "Admin")]
    public async Task WatchAll()
        => await Groups.AddToGroupAsync(Context.ConnectionId, "admin-tracking");

    /// <summary>
    /// Admin subscribes to a specific courier.
    /// CourierManager/Operator subscribe to their own courier (courierId ignored — taken from JWT).
    /// </summary>
    public async Task WatchCourier(int courierId)
    {
        var effectiveCourierId = ResolveEffectiveCourierId(courierId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"courier-{effectiveCourierId}");
    }

    /// <summary>Unsubscribe from a specific courier group.</summary>
    public async Task UnwatchCourier(int courierId)
    {
        var effectiveCourierId = ResolveEffectiveCourierId(courierId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"courier-{effectiveCourierId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin-tracking");
        await base.OnDisconnectedAsync(exception);
    }

    private int ResolveEffectiveCourierId(int requestedCourierId)
    {
        var isAdmin = Context.User?.IsInRole("Admin") ?? false;
        if (isAdmin) return requestedCourierId;

        // Courier roles: ignore requested id — use their own from JWT
        var courierIdClaim = Context.User?.FindFirstValue("CourierId");
        return courierIdClaim is not null ? int.Parse(courierIdClaim) : requestedCourierId;
    }
}
