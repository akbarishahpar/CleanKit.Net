using CleanKit.Net.Domain.Exceptions;
using CleanKit.Net.Domain.Primitives.Result;

namespace CleanKit.Net.Domain.Events;

public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await Execute(notification, cancellationToken);
        if (result.IsFailure)
            throw new DomainEventException(result.Error!);
    }

    protected virtual Task<Result> Execute(TDomainEvent notification, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}