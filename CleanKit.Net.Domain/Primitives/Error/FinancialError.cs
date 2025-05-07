namespace CleanKit.Net.Domain.Primitives.Error;

public class FinancialError : Error
{
    public FinancialError(string code, string message) : base(code, message)
    {
    }

    public FinancialError(string code, string message, params object[] args) : base(code, message, args)
    {
    }
}