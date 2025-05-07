using Framework.Application.Abstractions.Data;
using Framework.Domain.Events;
using Framework.Outbox.Abstractions.Repositories;
using Framework.Outbox.Entities;
using MediatR;

namespace Framework.Outbox.EventHandlers;

public class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly IConsumedOutboxMessagesRepository _consumedOutboxMessagesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IdempotentDomainEventHandler(
        INotificationHandler<TDomainEvent> decorated,
        IConsumedOutboxMessagesRepository consumedOutboxMessagesRepository,
        IUnitOfWork unitOfWork
    )
    {
        _decorated = decorated;
        _consumedOutboxMessagesRepository = consumedOutboxMessagesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var consumer = _decorated.GetType().Name;

        //Checking if the event has been consumed before or not
        var isConsumedBefore =
            await _consumedOutboxMessagesRepository.IsProcessedBeforeAsync(
                notification.Id,
                consumer,
                cancellationToken
            );

        if (isConsumedBefore)
            return;

        //Handling the event by actual notification handler
        await _decorated.Handle(notification, cancellationToken);

        //Inserting consumed event into database
        _consumedOutboxMessagesRepository.Add(new ConsumedOutboxMessage
        {
            EventId = notification.Id,
            Consumer = consumer
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}