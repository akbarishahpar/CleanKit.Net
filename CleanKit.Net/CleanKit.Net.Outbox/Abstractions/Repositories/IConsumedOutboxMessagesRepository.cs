using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Outbox.Entities;

namespace CleanKit.Net.Outbox.Abstractions.Repositories;

public interface IConsumedOutboxMessagesRepository : IGenericRepository<ConsumedOutboxMessage>
{
    Task<bool> IsProcessedBeforeAsync(Guid outboxMessageId, string consumer, CancellationToken cancellationToken);
}