namespace CleanKit.Net.Domain.Abstractions;

public interface ISoftDeletableEntity : IEntity
{
    public DateTime? DeletedOnUtc { get; set; }
    public bool IsDeleted { get; set; }
}
