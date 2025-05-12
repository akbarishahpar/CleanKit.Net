using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Outbox.Entities;

namespace CleanKit.Net.Outbox.Abstractions.Repositories;

public interface IOutboxMessagesRepository : IGenericRepository<OutboxMessage>
{
    Task<IEnumerable<OutboxMessage>> GetPendingOutboxMessagesAsync(CancellationToken cancellationToken);
}