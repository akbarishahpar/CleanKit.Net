namespace CleanKit.Net.Domain.Abstractions;

public interface IHasCreatedOnUtcEntity : IEntity
{
    public DateTime CreatedOnUtc { get; set; }
}