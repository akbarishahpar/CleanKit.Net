using CleanKit.Net.Domain.Primitives.Result;
using MediatR;

namespace CleanKit.Net.Application.Abstractions.Messaging;

public interface IQuery
{
}

public interface IQuery<TResponse> : IQuery, IRequest<Result<TResponse>>
{
}