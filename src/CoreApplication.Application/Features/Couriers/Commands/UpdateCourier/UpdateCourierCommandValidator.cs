using CoreApplication.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateCourier;

public class UpdateCourierCommandValidator : AbstractValidator<UpdateCourierCommand>
{
    public UpdateCourierCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SystemName)
            .MaximumLength(100)
            .MustAsync(async (cmd, systemName, ct) =>
                systemName == null || !await context.Couriers.AnyAsync(c => c.SystemName == systemName && c.Id != cmd.Id && !c.IsDeleted, ct))
            .WithMessage("System name already exists.");
        RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CodPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FreightCollectPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaximumCapacityPerDay).GreaterThan(0);
        RuleFor(x => x.MaximumShipmentWeight).GreaterThan(0);
        RuleFor(x => x.MaximumValueOfTheShipment).GreaterThan(0);
        RuleFor(x => x.MinimumParcelsInOneOrder).GreaterThan(0);
        RuleFor(x => x.SupportPhoneNumber).MaximumLength(20);
        RuleFor(x => x.Website).MaximumLength(200);
    }
}
