using CleanKit.Net.Idempotency.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Idempotency;

public static class DependencyInjection
{
    public static MediatRServiceConfiguration AddIdempotencyBehaviour(
        this MediatRServiceConfiguration options
    ) => options.AddOpenBehavior(typeof(IdempotencyBehaviour<,>));
}