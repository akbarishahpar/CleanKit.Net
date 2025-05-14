namespace CleanKit.Net.Domain.Primitives.Error;

public class DependencyError : Error
{
    public DependencyError(string code, string message) : base(code, message)
    {
    }

    public DependencyError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}