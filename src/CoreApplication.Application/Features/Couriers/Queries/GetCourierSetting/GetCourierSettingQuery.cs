using CoreApplication.Application.Features.Couriers.Common;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Queries.GetCourierSetting;

public record GetCourierSettingQuery(int CourierId) : IRequest<CourierSettingDto?>;
