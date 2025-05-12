using CleanKit.Net.Application;
using CleanKit.Net.Idempotency;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Template.Application;

public static class DependencyInjection
{
    public static void AddApplication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            options.AddLoggingBehaviour();
            options.AddValidationBehaviour();
            options.AddTransactionBehaviour();
            options.AddIdempotencyBehaviour();
        });

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true
        );

        // TODO: REGISTER OTHER REQUIRED SERVICES OR OPTIONS HERE
    }
}