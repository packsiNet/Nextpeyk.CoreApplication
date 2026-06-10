namespace CoreApplication.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("Access is forbidden.") { }
}
