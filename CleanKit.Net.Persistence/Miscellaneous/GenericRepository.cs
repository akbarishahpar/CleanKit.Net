using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Domain.Abstractions;
using CleanKit.Net.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanKit.Net.Persistence.Miscellaneous;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly IDatabaseContext DatabaseContext;
    protected readonly DbSet<TEntity> Entities;

    public GenericRepository(IDatabaseContext databaseContext)
    {
        DatabaseContext = databaseContext;
        Entities = DatabaseContext.Set<TEntity>();
    }

    #region Table

    protected IQueryable<TEntity> Table => typeof(TEntity).IsSubclassOf(typeof(ISoftDeletableEntity))
        ? Entities
            .Cast<ISoftDeletableEntity>()
            .Where(q => !q.IsDeleted)
            .Cast<TEntity>()
        : Entities;

    protected IQueryable<TEntity> NoTrackingTable => typeof(TEntity).IsSubclassOf(typeof(ISoftDeletableEntity))
        ? Entities
            .AsNoTracking()
            .Cast<ISoftDeletableEntity>()
            .Where(q => !q.IsDeleted)
            .Cast<TEntity>()
        : Entities;

    #endregion

    #region All & AllAsync

    public bool All(Expression<Func<TEntity, bool>> predicate)
        => Table.All(predicate);

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => Table.AllAsync(predicate, cancellationToken);

    #endregion

    #region Any & AnyAsync

    public bool Any(Expression<Func<TEntity, bool>> predicate)
        => Table.Any(predicate);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => Table.AnyAsync(predicate, cancellationToken);

    #endregion

    #region Add & AddRange

    public TEntity Add(TEntity entity)
    {
        Entities.Add(entity);
        return entity;
    }

    public void AddRange(IEnumerable<TEntity> entities) => Entities.AddRange(entities);

    #endregion

    #region ToList & ToListAsync

    public IList<TEntity> ToList(Expression<Func<TEntity, bool>> predicate)
        => Table.Where(predicate).ToList();

    public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => await Table.Where(predicate).ToListAsync(cancellationToken);

    public IList<TEntity> ToList()
        => Table.ToList();

    public async Task<IList<TEntity>> ToListAsync(CancellationToken cancellationToken)
        => await Table.ToListAsync(cancellationToken);

    #endregion

    #region Count & CountAsync

    public long Count(Expression<Func<TEntity, bool>> predicate)
        => Table.Where(predicate).Count();

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => await Table.Where(predicate).CountAsync(cancellationToken);

    public long Count()
        => Table.Count();

    public async Task<long> CountAsync(CancellationToken cancellationToken)
        => await Table.CountAsync(cancellationToken);

    #endregion

    #region Update & UpdateRange

    public void Update(TEntity entity) => Entities.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => Entities.UpdateRange(entities);

    #endregion

    #region Delete & DeleteRange

    public void Delete(TEntity entity) => Entities.Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities) => Entities.RemoveRange(entities);

    #endregion

    #region Attach & Detach

    public void Attach(TEntity entity) => Entities.Attach(entity);

    public void Detach(TEntity entity)
    {
        var entry = DatabaseContext.Entry(entity);
        entry.State = EntityState.Detached;
    }

    #endregion

    #region First & FirstAsync

    public TEntity First()
        => Table.First();

    public Task<TEntity> FirstAsync(CancellationToken cancellationToken)
        => Table.FirstAsync(cancellationToken);

    public TEntity First(Expression<Func<TEntity, bool>> predicate)
        => Table.First(predicate);

    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => Table.FirstAsync(predicate, cancellationToken);

    #endregion

    #region FirstOrDefault & FirstOrDefaultAsync

    public TEntity? FirstOrDefault()
        => Table.FirstOrDefault();

    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken)
        => Table.FirstOrDefaultAsync(cancellationToken);

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        => Table.FirstOrDefault(predicate);

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => Table.FirstOrDefaultAsync(predicate, cancellationToken);

    #endregion

    #region Last & LastAsync

    public TEntity Last()
        => Table.Last();

    public Task<TEntity> LastAsync(CancellationToken cancellationToken)
        => Table.LastAsync(cancellationToken);

    public TEntity Last(Expression<Func<TEntity, bool>> predicate)
        => Table.Last(predicate);

    public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => Table.LastAsync(predicate, cancellationToken);

    #endregion

    #region LastOrDefault & LastOrDefaultAsync

    public TEntity? LastOrDefault()
        => Table.LastOrDefault();

    public Task<TEntity?> LastOrDefaultAsync(CancellationToken cancellationToken) =>
        Table.LastOrDefaultAsync(cancellationToken);

    public TEntity? LastOrDefault(Expression<Func<TEntity, bool>> predicate)
        => Table.LastOrDefault(predicate);

    public Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => Table.LastOrDefaultAsync(predicate, cancellationToken);

    #endregion

    #region GetById & GetByIdAsync

    public TEntity? GetById(params object[] ids)
    {
        var entity = Entities.Find(ids);
        if (entity is ISoftDeletableEntity e)
            return e.IsDeleted ? null : entity;
        return entity;
    }

    public async Task<TEntity?> GetByIdAsync(
        CancellationToken cancellationToken,
        params object[] ids
    )
    {
        var entity = await Entities.FindAsync(ids, cancellationToken);
        if (entity is ISoftDeletableEntity e)
            return e.IsDeleted ? null : entity;
        return entity;
    }

    public Task<Paginated<TEntity>> PaginateAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool ascending,
        int page, int size, CancellationToken cancellationToken)
    {
        return ascending
            ? PaginateAscendingAsync(orderBy, page, size, cancellationToken)
            : PaginateDescendingAsync(orderBy, page, size, cancellationToken);
    }

    public Task<Paginated<TEntity>> PaginateAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page, int size,
        CancellationToken cancellationToken)
    {
        return ascending
            ? PaginateAscendingAsync(predicate, orderBy, page, size, cancellationToken)
            : PaginateDescendingAsync(predicate, orderBy, page, size, cancellationToken);
    }

    public Task<Paginated<TEntity>> PaginateAsync<TKey, TProperty>(Expression<Func<TEntity, TProperty>> loader,
        Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page,
        int size, CancellationToken cancellationToken)
    {
        return ascending
            ? PaginateAscendingAsync(loader, predicate, orderBy, page, size, cancellationToken)
            : PaginateDescendingAsync(loader, predicate, orderBy, page, size, cancellationToken);
    }

    public Task<Paginated<TEntity>> PaginateAsync<TKey, TProperty1, TProperty2>(Expression<Func<TEntity, TProperty1>> loader1, Expression<Func<TEntity, TProperty2>> loader2, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page, int size, CancellationToken cancellationToken)
    {
        return ascending
            ? PaginateAscendingAsync(loader1, loader2, predicate, orderBy, page, size, cancellationToken)
            : PaginateDescendingAsync(loader1, loader2, predicate, orderBy, page, size, cancellationToken);
    }

    public async Task<Paginated<TEntity>> PaginateAscendingAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy,
        int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.OrderBy(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateAscendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.Where(predicate).OrderBy(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateAscendingAsync<TKey, TProperty>(
        Expression<Func<TEntity, TProperty>> loader, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.Include(loader).Where(predicate).OrderBy(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateAscendingAsync<TKey, TProperty1, TProperty2>(Expression<Func<TEntity, TProperty1>> loader1, Expression<Func<TEntity, TProperty2>> loader2, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.Include(loader1).Include(loader2).Where(predicate).OrderBy(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).AsSplitQuery().ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateDescendingAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy,
        int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.OrderByDescending(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.Where(predicate).OrderByDescending(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateDescendingAsync<TKey, TProperty>(
        Expression<Func<TEntity, TProperty>> loader, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.Include(loader).Where(predicate).OrderByDescending(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public async Task<Paginated<TEntity>> PaginateDescendingAsync<TKey, TProperty1, TProperty2>(Expression<Func<TEntity, TProperty1>> loader1, Expression<Func<TEntity, TProperty2>> loader2, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = Table.Include(loader1).Include(loader2).Where(predicate).OrderByDescending(orderBy);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = await query.Skip((page - 1) * size)
            .Take(size).AsSplitQuery().ToListAsync(cancellationToken: cancellationToken);
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    #endregion

    #region LoadCollection

    public void LoadCollection<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty
    )
        where TProperty : class
    {
        Attach(entity);
        var collection = DatabaseContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            collection.Load();

        if (collection.CurrentValue == null) return;

        var collectionType = collection.CurrentValue.GetType();

        if (
            collectionType.IsGenericType
            && collectionType
                .GetGenericArguments()
                .First()
                .IsSubclassOf(typeof(ISoftDeletableEntity))
        )
        {
            collection.CurrentValue = collection.CurrentValue
                .Cast<ISoftDeletableEntity>()
                .Where(collectionEntity => !collectionEntity.IsDeleted)
                .Cast<TProperty>();
        }
    }

    public async Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<
            Func<TEntity, IEnumerable<TProperty>>
        > collectionProperty,
        CancellationToken cancellationToken
    )
        where TProperty : class
    {
        Attach(entity);

        var collection = DatabaseContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            await collection.LoadAsync(cancellationToken);

        if (collection.CurrentValue != null)
        {
            var collectionType = collection.CurrentValue.GetType();
            if (
                collectionType.IsGenericType
                && collectionType
                    .GetGenericArguments()
                    .First()
                    .IsSubclassOf(typeof(ISoftDeletableEntity))
            )
            {
                var castedCollection = collection.CurrentValue.Cast<ISoftDeletableEntity>();
                castedCollection = castedCollection.Where(
                    collectionEntity => !collectionEntity.IsDeleted
                );
                collection.CurrentValue = castedCollection.Cast<TProperty>().ToList();
            }
        }
    }

    #endregion

    #region Single & SingleAsync

    public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        => Table.Single(predicate);

    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => Table.SingleAsync(predicate, cancellationToken);

    #endregion

    #region SingleOrDefault & SingleOrDefaultAsync

    public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        => Table.SingleOrDefault(predicate);

    public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => Table.SingleOrDefaultAsync(predicate, cancellationToken);

    #endregion

    #region LoadReference & LoadReferenceAsync

    public void LoadReference<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty?>> referenceProperty
    )
        where TProperty : class
    {
        Attach(entity);
        var reference = DatabaseContext.Entry(entity).Reference(referenceProperty);
        if (!reference.IsLoaded)
            reference.Load();
        if (reference.CurrentValue is ISoftDeletableEntity { IsDeleted: true })
            reference.CurrentValue = null;
    }

    public async Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty?>> referenceProperty,
        CancellationToken cancellationToken
    )
        where TProperty : class
    {
        Attach(entity);
        var reference = DatabaseContext.Entry(entity).Reference(referenceProperty);
        if (!reference.IsLoaded)
            await reference.LoadAsync(cancellationToken);
        if (reference.CurrentValue is ISoftDeletableEntity { IsDeleted: true })
            reference.CurrentValue = null;
    }

    #endregion
}