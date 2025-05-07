using Framework.Domain.Abstractions;

namespace Framework.Outbox.Entities;

public class ConsumedOutboxMessage : IEntity
{
    public Guid EventId { get; set; }
    public string Consumer { get; set; } = string.Empty;
}