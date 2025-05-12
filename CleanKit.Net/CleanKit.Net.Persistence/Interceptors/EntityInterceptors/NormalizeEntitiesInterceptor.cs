using CleanKit.Net.Domain.Abstractions;
using CleanKit.Net.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace CleanKit.Net.Persistence.Interceptors.EntityInterceptors;

public class NormalizeEntitiesInterceptor : TypedSaveChangesInterceptor<IEntity>
{
    protected override void OnEntryObserved(EntityEntry<IEntity> entry)
    {
        if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
            return;

        var properties = entry.Entity
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p is { CanRead: true, CanWrite: true } && p.PropertyType == typeof(string));

        foreach (var property in properties)
        {
            var val = (string?)property.GetValue(entry.Entity, null);
            if (!val.HasValue()) continue;
            var newVal = StringExtensions.Normalize(val);
            if (newVal == val)
                continue;
            property.SetValue(entry.Entity, newVal, null);
        }
    }
}