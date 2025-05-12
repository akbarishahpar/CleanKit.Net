namespace CleanKit.Net.Domain.Primitives.Result;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    internal Result(TValue value, bool isSuccess, Error.Error? error)
        : base(isSuccess, error) => _value = value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}