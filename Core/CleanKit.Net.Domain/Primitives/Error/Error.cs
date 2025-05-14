using CleanKit.Net.Domain.Primitives.Result;

namespace CleanKit.Net.Domain.Primitives.Error;

public class Error : ValueObject
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public Error(string code, string message, params object[] args)
    {
        Code = code;
        Message = string.Format(message, args);
    }

    public static implicit operator string(Error? error) => error?.Code ?? string.Empty;
    public override string ToString()
    {
        return $"{Code}: {Message}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }
}