using MediatR;

namespace CoreApplication.Application.Features.FleetApp.Commands.RecordLocationBatch;

public record LocationPointDto(
    double Latitude,
    double Longitude,
    float? Accuracy,
    float? Speed,
    float? Heading,
    DateTime RecordedAt);

public record RecordLocationBatchCommand(List<LocationPointDto> Points) : IRequest;
