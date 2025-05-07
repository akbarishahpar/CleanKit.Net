using Framework.Outbox.BackgroundJobs;
using Framework.Outbox.EventHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Framework.Outbox;

public static class DependencyInjection
{
    public static void AddProcessOutboxMessagesJob(this IServiceCollectionQuartzConfigurator configurator)
    {
        var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
        configurator
            .AddJob<ProcessOutboxMessagesJob>(jobKey)
            .AddTrigger(trigger => trigger.ForJob(jobKey)
                .WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(5).RepeatForever()
                )
            );
    }

    public static IServiceCollection AddIdempotentDomainEventHandler(this IServiceCollection services)
        => services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
}