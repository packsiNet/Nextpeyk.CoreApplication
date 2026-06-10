using CoreApplication.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.AddCourierZone;

public class AddCourierZoneCommandValidator : AbstractValidator<AddCourierZoneCommand>
{
    public AddCourierZoneCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Radius).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
        RuleFor(x => x.CityId)
            .MustAsync(async (cityId, ct) =>
                cityId == null || await context.Cities.AnyAsync(c => c.Id == cityId && !c.IsDeleted, ct))
            .WithMessage("City not found.");
    }
}
