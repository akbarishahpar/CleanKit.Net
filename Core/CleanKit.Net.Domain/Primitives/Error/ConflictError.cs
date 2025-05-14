namespace CleanKit.Net.Domain.Primitives.Error;

public class ConflictError : Error
{
    public ConflictError(string code, string message) : base(code, message)
    {
    }

    public ConflictError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}