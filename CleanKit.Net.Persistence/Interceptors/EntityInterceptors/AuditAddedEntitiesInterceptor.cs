using CleanKit.Net.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanKit.Net.Persistence.Interceptors.EntityInterceptors;

public class AuditAddedEntitiesInterceptor : TypedSaveChangesInterceptor<IHasCreatedOnUtcEntity>
{
    protected override void OnEntryObserved(EntityEntry<IHasCreatedOnUtcEntity> entry)
    {
        if (
            entry.State == EntityState.Added &&
            entry.Entity.CreatedOnUtc == default
        ) entry.Entity.CreatedOnUtc = DateTime.UtcNow;
    }
}