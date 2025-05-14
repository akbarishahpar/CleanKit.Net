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
        typeof(CleanKit.Net.Idempotency.DependencyInjection).Assembly,
        typeof(CleanKit.Net.Idempotency.Persistence.DependencyInjection).Assembly,
        typeof(CleanKit.Net.Outbox.DependencyInjection).Assembly,
        typeof(CleanKit.Net.Outbox.Persistence.DependencyInjection).Assembly,
        typeof(Domain.DependencyInjection).Assembly, // Project-specific domain
        typeof(DependencyInjection).Assembly 
    ];
}