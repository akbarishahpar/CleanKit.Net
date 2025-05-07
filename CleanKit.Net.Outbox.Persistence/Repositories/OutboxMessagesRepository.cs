using Framework.Application.Abstractions.Data;
using Framework.Outbox.Abstractions.Repositories;
using Framework.Outbox.Entities;
using Framework.Outbox.Enumerations;
using Framework.Persistence.Abstractions;
using Framework.Persistence.Miscellaneous;
using Microsoft.EntityFrameworkCore;

namespace Framework.Outbox.Persistence.Repositories;

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