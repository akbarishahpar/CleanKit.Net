using CleanKit.Net.Domain.Abstractions;

namespace CleanKit.Net.Idempotency.Entities;

public class IdempotentRequest : IHasCreatedOnUtcEntity
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}