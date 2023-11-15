namespace Shared.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() { }

    public ForbiddenException(string message)
        : base(message) { }
}
