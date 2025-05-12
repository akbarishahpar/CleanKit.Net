using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Idempotency.Persistence;
using CleanKit.Net.Outbox;
using CleanKit.Net.Outbox.Persistence;
using CleanKit.Net.Persistence;
using CleanKit.Net.Persistence.Abstractions;
using CleanKit.Net.Template.Persistence.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Template.Persistence;

public class DependencyInjection
{
    public static IServiceCollection AddPersistence(IServiceCollection services,
        IConfiguration configuration)
    {
        // Getting connection string for main database
        var connectionString = configuration.GetConnectionString("MainDb"); // TODO: MAIN CONNECTION STRING NAME GOES HERE

        // Registering main database context and its options
        services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(connectionString)
                .AddDefaultInterceptors()
                .AddOutboxMessagesInterceptor()
        );
        services.AddScoped<IDatabaseContext>(sp => sp.GetRequiredService<MainDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MainDbContext>());
        
        // Adding Outbox repositories
        services.AddOutboxRepositories();

        // Adding Idempotency repositories
        services.AddIdempotencyRepositories();

        // Making domain event handlers idempotent 
        services.AddIdempotentDomainEventHandler();

        // TODO: REGISTER REPOSITORIES HERE
        
        return services;
    }
}