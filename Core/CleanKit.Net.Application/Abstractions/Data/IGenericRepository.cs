using System.Linq.Expressions;
using CleanKit.Net.Domain.Abstractions;

namespace CleanKit.Net.Application.Abstractions.Data;

public interface IGenericRepository<TEntity>
    where TEntity : IEntity
{
    bool All(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    bool Any(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Attach(TEntity entity);

    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    void Detach(TEntity entity);

    TEntity First();
    Task<TEntity> FirstAsync(CancellationToken cancellationToken);
    TEntity First(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity? FirstOrDefault();
    Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken);
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity Last();
    Task<TEntity> LastAsync(CancellationToken cancellationToken);
    TEntity Last(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity? LastOrDefault();
    Task<TEntity?> LastOrDefaultAsync(CancellationToken cancellationToken);
    TEntity? LastOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity? GetById(params object[] ids);
    Task<TEntity?> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    Task<Paginated<TEntity>> PaginateAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page,
        int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page, int size,
        CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAsync<TKey, TProperty>(
        Expression<Func<TEntity, TProperty>> loader, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page, int size,
        CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAsync<TKey, TProperty1, TProperty2>(
        Expression<Func<TEntity, TProperty1>> loader1, Expression<Func<TEntity, TProperty2>> loader2, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, bool ascending, int page, int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAscendingAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, int page, int size,
        CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAscendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAscendingAsync<TKey, TProperty>(
        Expression<Func<TEntity, TProperty>> loader, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateAscendingAsync<TKey, TProperty1, TProperty2>(
        Expression<Func<TEntity, TProperty1>> loader1, Expression<Func<TEntity, TProperty2>> loader2, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateDescendingAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, int page, int size,
        CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateDescendingAsync<TKey, TProperty>(
        Expression<Func<TEntity, TProperty>> loader, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken);

    Task<Paginated<TEntity>> PaginateDescendingAsync<TKey, TProperty1, TProperty2>(
        Expression<Func<TEntity, TProperty1>> loader1, Expression<Func<TEntity, TProperty2>> loader2, Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy, int page, int size, CancellationToken cancellationToken);

    void LoadCollection<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty
    )
        where TProperty : class;

    Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty,
        CancellationToken cancellationToken
    )
        where TProperty : class;

    void LoadReference<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty?>> referenceProperty
    )
        where TProperty : class;

    Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty?>> referenceProperty,
        CancellationToken cancellationToken
    )
        where TProperty : class;

    IList<TEntity> ToList(Expression<Func<TEntity, bool>> predicate);
    Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    IList<TEntity> ToList();
    Task<IList<TEntity>> ToListAsync(CancellationToken cancellationToken);

    long Count(Expression<Func<TEntity, bool>> predicate);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    long Count();
    Task<long> CountAsync(CancellationToken cancellationToken);

    TEntity Single(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
}