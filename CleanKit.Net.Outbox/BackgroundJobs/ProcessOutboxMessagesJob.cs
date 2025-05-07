using Framework.Application.Abstractions.Data;
using Framework.Domain.Events;
using Framework.Outbox.Abstractions.Repositories;
using Framework.Outbox.Enumerations;
using MediatR;
using Newtonsoft.Json;
using Polly;
using Quartz;

namespace Framework.Outbox.BackgroundJobs;

[DisallowConcurrentExecution]
// ReSharper disable once ClassNeverInstantiated.Global
public class ProcessOutboxMessagesJob : IJob
{
    private readonly IOutboxMessagesRepository _outboxMessagesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(
        IOutboxMessagesRepository outboxMessagesRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    )
    {
        _outboxMessagesRepository = outboxMessagesRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //Define retry policy
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                4,
                attempt => TimeSpan.FromMicroseconds(250 * attempt)
            );

        //Fetching pending OutboxMessages
        var outboxMessages = (
            await _outboxMessagesRepository
                .GetPendingOutboxMessagesAsync(context.CancellationToken)
        ).ToArray();

        //Publishing fetched OutboxMessages one by one
        foreach (var outboxMessage in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            var result =
                await policy.ExecuteAndCaptureAsync(() => _publisher.Publish(domainEvent, context.CancellationToken));

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;

            if (result.FinalException is null)
            {
                outboxMessage.State = OutboxMessageState.Succeeded;
            }
            else
            {
                outboxMessage.State = OutboxMessageState.Failed;
                outboxMessage.Error = result.FinalException?.ToString();
            }
        }

        if (outboxMessages.Any())
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}