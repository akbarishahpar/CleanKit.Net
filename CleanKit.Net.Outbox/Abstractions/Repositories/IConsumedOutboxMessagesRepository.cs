using Framework.Application.Abstractions.Data;
using Framework.Outbox.Entities;

namespace Framework.Outbox.Abstractions.Repositories;

public interface IConsumedOutboxMessagesRepository : IGenericRepository<ConsumedOutboxMessage>
{
    Task<bool> IsProcessedBeforeAsync(Guid outboxMessageId, string consumer, CancellationToken cancellationToken);
}