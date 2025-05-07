using CleanKit.Net.Domain.Primitives.Result;
using MediatR;

namespace CleanKit.Net.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface INonTransactional
{
}