using CleanKit.Net.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanKit.Net.Persistence.Interceptors.EntityInterceptors;

public class AuditModifiedEntitiesInterceptor : TypedSaveChangesInterceptor<IAuditableEntity>
{
    protected override void OnEntryObserved(EntityEntry<IAuditableEntity> entry)
    {
        if (entry.State == EntityState.Modified)
            entry.Entity.ModifiedOnUtc = DateTime.UtcNow;
    }
}