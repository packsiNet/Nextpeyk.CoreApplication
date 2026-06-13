using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Commands.RevertAbsentParcel;

public class RevertAbsentParcelCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<RevertAbsentParcelCommand>
{
    public async Task Handle(RevertAbsentParcelCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();
        var userId = currentUser.UserId ?? 0;
        var now = dateTime.UtcNow;

        var parcel = await context.ParcelCouriers
            .FirstOrDefaultAsync(pc =>
                pc.Id == request.ParcelCourierId &&
                pc.CourierId == courierId &&
                pc.Status == ParcelTrackingEnum.PhysicallyAbsent &&
                !pc.IsDeleted,
                cancellationToken)
            ?? throw new NotFoundException(nameof(ParcelCourier), request.ParcelCourierId);

        parcel.Status = ParcelTrackingEnum.Processing;
        parcel.ModifiedByUserId = userId;
        parcel.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}
