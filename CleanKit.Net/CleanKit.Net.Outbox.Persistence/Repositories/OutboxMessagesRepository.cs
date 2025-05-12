using CleanKit.Net.Outbox.Abstractions.Repositories;
using CleanKit.Net.Outbox.Entities;
using CleanKit.Net.Outbox.Enumerations;
using CleanKit.Net.Persistence.Abstractions;
using CleanKit.Net.Persistence.Miscellaneous;
using Microsoft.EntityFrameworkCore;

namespace CleanKit.Net.Outbox.Persistence.Repositories;

public class OutboxMessagesRepository : GenericRepository<OutboxMessage>, IOutboxMessagesRepository
{
    public OutboxMessagesRepository(IDatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public async Task<IEnumerable<OutboxMessage>> GetPendingOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        const int chunkSize = 20;
        return await Table
            .Where(outboxMessage => outboxMessage.State == OutboxMessageState.Pending)
            .OrderBy(outboxMessage => outboxMessage.OccurredOnUtc)
            .Take(chunkSize)
            .ToListAsync(cancellationToken);
    }
}