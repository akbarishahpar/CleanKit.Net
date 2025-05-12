using CleanKit.Net.Domain.Primitives.Result;
using MediatR;

namespace CleanKit.Net.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}