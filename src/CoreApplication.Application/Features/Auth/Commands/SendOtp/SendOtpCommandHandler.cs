using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Auth.Commands.SendOtp;

public class SendOtpCommandHandler(
    IApplicationDbContext context,
    ISmsSender smsSender,
    IDateTimeService dateTime)
    : IRequestHandler<SendOtpCommand>
{
    private const int OtpExpiryMinutes = 5;
    private const int OtpLength = 6;

    public async Task Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var userExists = await context.UserAccounts
            .AnyAsync(u => u.PhoneNumber == request.PhoneNumber && u.IsActive && !u.IsDeleted,
                cancellationToken);

        if (!userExists)
            throw new NotFoundException("UserAccount", request.PhoneNumber);

        // Invalidate existing unused OTPs for this phone
        var existingOtps = await context.OtpCodes
            .Where(o => o.PhoneNumber == request.PhoneNumber && !o.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (var otp in existingOtps)
            otp.IsUsed = true;

        var code = GenerateCode();
        context.OtpCodes.Add(new OtpCode
        {
            PhoneNumber = request.PhoneNumber,
            Code = code,
            ExpiresAt = dateTime.UtcNow.AddMinutes(OtpExpiryMinutes),
            IsUsed = false,
            CreatedAt = dateTime.UtcNow
        });

        await context.SaveChangesAsync(cancellationToken);
        await smsSender.SendAsync(request.PhoneNumber, $"کد ورود شما: {code}", cancellationToken);
    }

    private static string GenerateCode()
    {
        return Random.Shared.Next(100_000, 999_999).ToString();
    }
}
