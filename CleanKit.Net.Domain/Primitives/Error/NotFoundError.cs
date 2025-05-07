namespace CleanKit.Net.Domain.Primitives.Error;

public class NotFoundError : Error
{
    public NotFoundError(string code, string message) : base(code, message)
    {
    }

    public NotFoundError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}