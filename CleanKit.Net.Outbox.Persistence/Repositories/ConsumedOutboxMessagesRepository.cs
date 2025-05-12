using CleanKit.Net.Outbox.Abstractions.Repositories;
using CleanKit.Net.Outbox.Entities;
using CleanKit.Net.Persistence.Abstractions;
using CleanKit.Net.Persistence.Miscellaneous;

namespace CleanKit.Net.Outbox.Persistence.Repositories;

public class ConsumedOutboxMessagesRepository : GenericRepository<ConsumedOutboxMessage>,
    IConsumedOutboxMessagesRepository
{
    public ConsumedOutboxMessagesRepository(IDatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public async Task<bool> IsProcessedBeforeAsync(Guid outboxMessageId, string consumer,
        CancellationToken cancellationToken)
    {
        return await AnyAsync(outboxMessageConsumer =>
            outboxMessageConsumer.EventId == outboxMessageId &&
            outboxMessageConsumer.Consumer == consumer, cancellationToken);
    }
}