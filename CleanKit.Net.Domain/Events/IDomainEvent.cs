using MediatR;

namespace CleanKit.Net.Domain.Events;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
}
