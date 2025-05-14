namespace CleanKit.Net.Domain.Primitives.Error;

public class ValidationError : Error
{
    public ValidationError(string code, string message) : base(code, message)
    {
    }

    public ValidationError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}