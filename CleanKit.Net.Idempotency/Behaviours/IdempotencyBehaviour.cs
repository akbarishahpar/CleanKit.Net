using System.Text.Json;
using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Domain.Primitives.Error;
using CleanKit.Net.Domain.Primitives.Result;
using CleanKit.Net.Idempotency.Abstractions.Primitives;
using CleanKit.Net.Idempotency.Abstractions.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanKit.Net.Idempotency.Behaviours;

public class IdempotencyBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IIdempotentCommand
    where TResponse : Result
{
    private readonly IIdempotentRequestsRepository _idempotentRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IdempotencyBehaviour<TRequest, TResponse>> _logger;

    public IdempotencyBehaviour(IIdempotentRequestsRepository idempotentRequestsRepository, IUnitOfWork unitOfWork, ILogger<IdempotencyBehaviour<TRequest, TResponse>> logger)
    {
        _idempotentRequestsRepository = idempotentRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        //Checking if given request id has been processed before or not
        if (string.IsNullOrEmpty(request.RequestId) || await _idempotentRequestsRepository.RequestExistsAsync(request.RequestId, cancellationToken))
        {
            _logger.LogWarning("Duplicate request id for an idempotent command received ({@RequestId}, {@RequestBody})", request.RequestId, request);
            var error = new BadRequestError("Idempotency.DuplicateRequestId", "Given request id is processed before.");
            if (typeof(TResponse).IsGenericType)
                return (TResponse)Activator.CreateInstance(typeof(TResponse), null, false, error)!;
            return (TResponse)Activator.CreateInstance(typeof(TResponse), false, error)!;
        }

        //Inserting newly received request id into database
        _idempotentRequestsRepository.CreateRequest(request.RequestId, request);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //Processing next step of the pipeline
        var response = await next();

        //Returning the response
        return response;
    }
}