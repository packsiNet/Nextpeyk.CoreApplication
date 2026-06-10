using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Parcels.Commands.RegisterParcel;

public class RegisterParcelCommandHandler(
    IApplicationDbContext context,
    ISenderContext senderContext,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<RegisterParcelCommand, int>
{
    public async Task<int> Handle(RegisterParcelCommand request, CancellationToken cancellationToken)
    {
        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var parcel = new Parcel
        {
            Barcode = GenerateBarcode(now),
            ParcelType = request.ParcelType,
            Description = request.Description,
            Length = request.Length,
            Width = request.Width,
            Height = request.Height,
            Weight = request.Weight,
            Value = request.Value,
            SenderFirstName = request.SenderFirstName,
            SenderLastName = request.SenderLastName,
            SenderPhoneNumber = request.SenderPhoneNumber,
            SenderLatitude = request.SenderLatitude,
            SenderLongitude = request.SenderLongitude,
            SenderAddress = request.SenderAddress,
            ReceiverFirstName = request.ReceiverFirstName,
            ReceiverLastName = request.ReceiverLastName,
            ReceiverPhoneNumber = request.ReceiverPhoneNumber,
            ReceiverLatitude = request.ReceiverLatitude,
            ReceiverLongitude = request.ReceiverLongitude,
            ReceiverAddress = request.ReceiverAddress,
            HasCOD = request.HasCOD,
            HasFMCG = request.HasFMCG,
            HasFreightCollect = request.HasFreightCollect,
            State = ParcelState.Registered,
            SenderId = senderContext.SenderId,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.Parcels.Add(parcel);
        await context.SaveChangesAsync(cancellationToken);

        return parcel.Id;
    }

    private static string GenerateBarcode(DateTime now)
        => $"PKG-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
}
