using CleanKit.Net.Outbox.Abstractions.Repositories;
using CleanKit.Net.Outbox.Persistence.Interceptors;
using CleanKit.Net.Outbox.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Outbox.Persistence;

public static class DependencyInjection
{
    public static DbContextOptionsBuilder AddOutboxMessagesInterceptor(this DbContextOptionsBuilder options)
        => options
            .AddInterceptors(new ConvertDomainEventsToOutboxMessagesInterceptor());

    public static IServiceCollection AddOutboxRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOutboxMessagesRepository, OutboxMessagesRepository>();
        services.AddScoped<IConsumedOutboxMessagesRepository, ConsumedOutboxMessagesRepository>();
        return services;
    }
}