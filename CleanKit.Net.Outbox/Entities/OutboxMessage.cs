using Framework.Domain.Abstractions;
using Framework.Outbox.Enumerations;

namespace Framework.Outbox.Entities;

public class OutboxMessage : IEntity
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public OutboxMessageState State { get; set; }
    public string? Error { get; set; }
}