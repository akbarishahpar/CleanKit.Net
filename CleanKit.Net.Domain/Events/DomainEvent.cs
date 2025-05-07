namespace CleanKit.Net.Domain.Events;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; }

    protected DomainEvent()
        => Id = Guid.NewGuid();
}