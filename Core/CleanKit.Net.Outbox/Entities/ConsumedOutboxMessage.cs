using CleanKit.Net.Domain.Abstractions;

namespace CleanKit.Net.Outbox.Entities;

public class ConsumedOutboxMessage : IEntity
{
    public Guid EventId { get; set; }
    public string Consumer { get; set; } = string.Empty;
}