namespace CleanKit.Net.Domain.Primitives.Error;

public class BadRequestError : Error
{
    public BadRequestError(string code, string message) : base(code, message)
    {
    }

    public BadRequestError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}