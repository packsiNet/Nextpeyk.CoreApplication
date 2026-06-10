namespace CoreApplication.Domain.Enums;

public enum ReturnParcelReason
{
    WrongAddress = 0,
    RecipientNotAvailable = 1,
    WaitedOverFiveMinutes = 2,
    RecipientRefused = 3,
    PackageDamaged = 4,
    Other = 5
}
