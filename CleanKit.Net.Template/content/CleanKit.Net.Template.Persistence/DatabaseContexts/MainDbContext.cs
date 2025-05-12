using CleanKit.Net.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CleanKit.Net.Template.Persistence.DatabaseContexts;

public class MainDbContext : DatabaseContext
{
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) => EntitiesAssemblies =
    [
        typeof(Idempotency.DependencyInjection).Assembly,
        typeof(Idempotency.Persistence.DependencyInjection).Assembly,
        typeof(Outbox.DependencyInjection).Assembly,
        typeof(Outbox.Persistence.DependencyInjection).Assembly,
        typeof(Domain.DependencyInjection).Assembly, // Project-specific domain
        typeof(DependencyInjection).Assembly
    ];
}