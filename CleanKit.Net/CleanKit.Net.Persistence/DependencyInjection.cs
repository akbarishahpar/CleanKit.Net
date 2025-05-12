using CleanKit.Net.Persistence.Interceptors.EntityInterceptors;
using Microsoft.EntityFrameworkCore;

namespace CleanKit.Net.Persistence;

public static class DependencyInjection
{
    public static DbContextOptionsBuilder AddDefaultInterceptors(this DbContextOptionsBuilder options)
        => options
            .AddInterceptors(new AuditAddedEntitiesInterceptor())
            .AddInterceptors(new AuditModifiedEntitiesInterceptor())
            .AddInterceptors(new NormalizeEntitiesInterceptor())
            .AddInterceptors(new SoftDeleteEntitiesInterceptor());
}