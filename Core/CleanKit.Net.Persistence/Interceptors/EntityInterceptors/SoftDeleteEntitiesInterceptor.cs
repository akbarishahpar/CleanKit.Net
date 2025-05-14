using CleanKit.Net.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanKit.Net.Persistence.Interceptors.EntityInterceptors;

public class SoftDeleteEntitiesInterceptor : TypedSaveChangesInterceptor<ISoftDeletableEntity>
{
    protected override void OnEntryObserved(EntityEntry<ISoftDeletableEntity> entry)
    {
        if (entry.State == EntityState.Deleted)
        {
            entry.State = EntityState.Modified;
            entry.Entity.DeletedOnUtc = DateTime.UtcNow;
            entry.Entity.IsDeleted = true;
        }
    }
}