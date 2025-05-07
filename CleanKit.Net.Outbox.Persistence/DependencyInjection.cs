using Framework.Outbox.Abstractions.Repositories;
using Framework.Outbox.Persistence.Interceptors;
using Framework.Outbox.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Outbox.Persistence;

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