using CleanKit.Net.Domain.Primitives.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CleanKit.Net.Utils;

namespace CleanKit.Net.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
        where TResponse : Result
    {
        private readonly ILogger<IPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<IPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            //Setting up the stopwatch
            var sp = new Stopwatch();
            sp.Start();

            //Logging start of the request
            _logger.LogDebug(
                "{@RequestType} request started at {@DateTimeUtc}",
                typeof(TRequest).Name,
                DateTime.UtcNow
            );

            var result = await next(); //Executing the next part of the pipeline

            if (result.IsFailure)
            {
                //Logging failure of the request
                _logger.LogWarning(
                    "{@RequestType} request failed at {@DateTimeUtc} in {@ElapsedMilliseconds}ms (ErrorCode={@ErrorCode}, ErrorMessage={@ErrorMessage})",
                    typeof(TRequest).Name,
                    DateTime.UtcNow,
                    sp.ElapsedMilliseconds,
                    (result.Error?.Code).EmptyIfNull(),
                    (result.Error?.Message).EmptyIfNull()
                );
            }
            else
            {
                //Logging completeness of the request
                _logger.LogDebug(
                    "{@RequestType} request completed at {@DateTimeUtc} in {@ElapsedMilliseconds}ms",
                    typeof(TRequest).Name,
                    DateTime.UtcNow,
                    sp.ElapsedMilliseconds
                );
            }

            //Returning the final result
            return result;
        }
    }
}
