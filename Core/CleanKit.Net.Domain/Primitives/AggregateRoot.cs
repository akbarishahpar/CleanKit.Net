using CleanKit.Net.Domain.Events;

namespace CleanKit.Net.Domain.Primitives;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    protected AggregateRoot(Guid id)
        : base(id) { }

    protected AggregateRoot() { }

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
