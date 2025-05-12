using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanKit.Net.Persistence.Interceptors;

public class TypedSaveChangesInterceptor<T> : SaveChangesInterceptor where T : class
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Iterate(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Iterate(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void Iterate(DbContext? dbContext)
    {
        if (dbContext is null)
            return;
        foreach (var entry in dbContext.ChangeTracker.Entries<T>())
            OnEntryObserved(entry);
    }

    protected virtual void OnEntryObserved(EntityEntry<T> entry)
        => throw new NotImplementedException();
}