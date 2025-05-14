using CleanKit.Net.Idempotency.Abstractions.Repositories;
using CleanKit.Net.Idempotency.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Idempotency.Persistence;

public static class DependencyInjection
{
    public static void AddIdempotencyRepositories(this IServiceCollection services)
    {
        services.AddScoped<IIdempotentRequestsRepository, IdempotentRequestsRepository>();
    }
}