using Framework.Application.Abstractions.Data;
using Framework.Outbox.Entities;

namespace Framework.Outbox.Abstractions.Repositories;

public interface IOutboxMessagesRepository : IGenericRepository<OutboxMessage>
{
    Task<IEnumerable<OutboxMessage>> GetPendingOutboxMessagesAsync(CancellationToken cancellationToken);
}