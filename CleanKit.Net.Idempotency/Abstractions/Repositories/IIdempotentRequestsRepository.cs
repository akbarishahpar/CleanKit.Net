using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Idempotency.Entities;

namespace CleanKit.Net.Idempotency.Abstractions.Repositories;

public interface IIdempotentRequestsRepository : IGenericRepository<IdempotentRequest>
{
    Task<bool> RequestExistsAsync(string requestId, CancellationToken cancellationToken);
    void CreateRequest<T>(string requestId, T body);
}