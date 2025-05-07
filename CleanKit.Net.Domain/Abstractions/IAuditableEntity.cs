namespace CleanKit.Net.Domain.Abstractions;

public interface IAuditableEntity : IHasCreatedOnUtcEntity
{
    public DateTime? ModifiedOnUtc { get; set; }
}