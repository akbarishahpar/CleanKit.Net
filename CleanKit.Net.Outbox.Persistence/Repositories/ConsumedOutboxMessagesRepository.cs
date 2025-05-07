using Framework.Outbox.Abstractions.Repositories;
using Framework.Outbox.Entities;
using Framework.Persistence.Abstractions;
using Framework.Persistence.Miscellaneous;

namespace Framework.Outbox.Persistence.Repositories;

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