namespace CleanKit.Net.Application.Abstractions.Data;

public class Paginated<T>
{
    public long Total { get; set; }
    public long Pages { get; set; }
    public List<T> Items { get; set; } = new();

    public Paginated<TProjection> ProjectTo<TProjection>(Func<T, TProjection> predicate)
    {
        return new Paginated<TProjection>
        {
            Total = Total,
            Pages = Pages,
            Items = Items.Select(predicate).ToList()
        };
    }
}