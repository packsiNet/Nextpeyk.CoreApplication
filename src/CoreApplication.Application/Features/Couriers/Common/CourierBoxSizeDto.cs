namespace CoreApplication.Application.Features.Couriers.Common;

public record CourierBoxSizeDto(
    int Id,
    int BoxSizeId,
    string BoxSizeTitle,
    string BoxSizeCode,
    decimal PackagingPrice);
