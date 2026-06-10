namespace CoreApplication.Application.Features.Couriers.Common;

public record BoxSizeDto(
    int Id,
    int Order,
    string Title,
    string Code,
    decimal BoxLength,
    decimal BoxWidth,
    decimal BoxHeight);
