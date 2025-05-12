using CleanKit.Net.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanKit.Net.Persistence.Abstractions
{
    public interface IDatabaseContext : IUnitOfWork
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
