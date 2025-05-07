using CleanKit.Net.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanKit.Net.Persistence.Extensions;

public static class QueryableExtensions
{
    public static async Task<Paginated<TEntity>> PaginateAsync<TEntity, TKey>(
        this IQueryable<TEntity> dataSource,
        Expression<Func<TEntity, TKey>> orderBy,
        int page,
        int size,
        bool ascending,
        CancellationToken cancellationToken
    )
    {
        var query = ascending
            ? dataSource.OrderBy(orderBy)
            : dataSource.OrderByDescending(orderBy);
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
}