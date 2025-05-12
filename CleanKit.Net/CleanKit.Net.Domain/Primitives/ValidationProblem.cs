namespace CleanKit.Net.Domain.Primitives;

public class ValidationProblem : ValueObject
{
    public string Property { get; }
    public string Code { get; set; }
    public string Message { get; }

    public ValidationProblem(string property, string code, string message)
    {
        Property = property;
        Code = code;
        Message = message;
    }

    public ValidationProblem(string property, string code, string message, params object[] args)
    {
        Property = property;
        Code = code;
        Message = string.Format(message, args);
    }

    public static implicit operator string(ValidationProblem error) => error.Property;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Property;
        yield return Code;
        yield return Message;
    }
}