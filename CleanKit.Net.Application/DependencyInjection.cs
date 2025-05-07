using CleanKit.Net.Application.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Application;

public static class DependencyInjection
{
    public static MediatRServiceConfiguration AddLoggingBehaviour(
        this MediatRServiceConfiguration options
    ) => options.AddOpenBehavior(typeof(LoggingBehaviour<,>));

    public static MediatRServiceConfiguration AddValidationBehaviour(
        this MediatRServiceConfiguration options
    ) => options.AddOpenBehavior(typeof(ValidationBehaviour<,>));

    public static MediatRServiceConfiguration AddTransactionBehaviour(
        this MediatRServiceConfiguration options
    ) => options.AddOpenBehavior(typeof(TransactionBehaviour<,>));
}
