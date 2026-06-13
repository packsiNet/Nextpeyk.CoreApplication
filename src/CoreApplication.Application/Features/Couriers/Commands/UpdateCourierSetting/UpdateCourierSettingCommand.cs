using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateCourierSetting;

public record UpdateCourierSettingCommand(int CourierId, bool IsGroupAcceptance) : IRequest;
