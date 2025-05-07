using CleanKit.Net.Idempotency.Abstractions.Repositories;
using CleanKit.Net.Idempotency.Entities;
using CleanKit.Net.Persistence.Abstractions;
using CleanKit.Net.Persistence.Miscellaneous;
using Microsoft.EntityFrameworkCore;

namespace CleanKit.Net.Idempotency.Persistence.Repositories;

public class IdempotentRequestsRepository : GenericRepository<IdempotentRequest>, IIdempotentRequestsRepository
{
    public IdempotentRequestsRepository(IDatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public async Task<bool> RequestExistsAsync(string requestId, CancellationToken cancellationToken)
    {
        return await Table.AnyAsync(request => request.Id == requestId, cancellationToken);
    }

    public void CreateRequest<T>(string requestId, T body)
    {
        Add(new IdempotentRequest
        {
            Id = requestId,
            CreatedOnUtc = DateTime.UtcNow,
            Name = typeof(T).Name,
            Body = System.Text.Json.JsonSerializer.Serialize(body)
        });
    }
}