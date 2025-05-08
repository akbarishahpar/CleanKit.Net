using System.Text.Json.Serialization;

namespace CleanKit.Net.Domain.Primitives.Result;

public class Result
{
    public bool IsSuccess { get; }

    [JsonIgnore] public bool IsFailure => !IsSuccess;

    public Error.Error? Error { get; }

    internal Result(bool isSuccess, Error.Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(Error.Error? error) => new(false, error);

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (var result in results.Where(x => x.IsFailure))
            return Failure(result.Error);
        return Success();
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);

    public static Result<TValue> Failure<TValue>(Error.Error? error) => new(default!, false, error);

    public static Result<TValue> FirstFailureOrSuccess<TValue>(TValue value, params Result[] results)
    {
        foreach (var result in results.Where(x => x.IsFailure))
            return Failure<TValue>(result.Error);
        return Success(value);
    }
}