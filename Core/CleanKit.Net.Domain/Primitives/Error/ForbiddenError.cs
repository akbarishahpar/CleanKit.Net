namespace CleanKit.Net.Domain.Primitives.Error;

public class ForbiddenError : Error
{
    public ForbiddenError(string code, string message) : base(code, message)
    {
    }

    public ForbiddenError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}