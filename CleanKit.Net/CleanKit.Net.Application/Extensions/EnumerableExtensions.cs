using CleanKit.Net.Application.Abstractions.Data;

namespace CleanKit.Net.Persistence.Extensions;

public static class EnumerableExtensions
{
    public static Paginated<TEntity> Paginate<TEntity, TKey>(
        this IEnumerable<TEntity> dataSource,
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> orderBy,
        bool ascending,
        int page,
        int size
    )
    {
        return ascending
            ? PaginateAscending(dataSource, predicate, orderBy, page, size)
            : PaginateDescending(dataSource, predicate, orderBy, page, size);
    }

    public static Paginated<TEntity> PaginateAscending<TEntity, TKey>(
        this IEnumerable<TEntity> dataSource,
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> orderBy,
        int page,
        int size
    )
    {
        var query = dataSource.Where(predicate).OrderBy(orderBy).ToList();
        var total = query.Count;
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = query.Skip((page - 1) * size).Take(size).ToList();
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }

    public static Paginated<TEntity> PaginateDescending<TEntity, TKey>(
        this IEnumerable<TEntity> dataSource,
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> orderBy,
        int page,
        int size
    )
    {
        var query = dataSource.Where(predicate).OrderByDescending(orderBy).ToList();
        var total = query.Count;
        var pages = (long)Math.Ceiling(total / (double)size);
        var items = query.Skip((page - 1) * size).Take(size).ToList();
        return new Paginated<TEntity>
        {
            Total = total,
            Pages = pages,
            Items = items
        };
    }
}