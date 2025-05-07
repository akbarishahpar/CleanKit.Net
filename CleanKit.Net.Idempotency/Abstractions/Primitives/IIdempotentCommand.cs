namespace CleanKit.Net.Idempotency.Abstractions.Primitives;

public interface IIdempotentCommand
{
    public string RequestId { get; set; }
}