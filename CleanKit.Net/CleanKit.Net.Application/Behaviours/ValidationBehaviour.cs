using FluentValidation;
using CleanKit.Net.Application.Abstractions.Messaging;
using CleanKit.Net.Domain.Primitives;
using CleanKit.Net.Domain.Primitives.Result;
using MediatR;

namespace CleanKit.Net.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (request is IQuery<TRequest>)
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators
            .Select(async validator => await validator.ValidateAsync(context, cancellationToken)));
        var validationProblems = validationResults
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(validationFailure => new ValidationProblem(
                validationFailure.PropertyName,
                validationFailure.ErrorCode,
                validationFailure.ErrorMessage
            ))
            .Distinct()
            .ToArray();

        if (validationProblems.Any())
            return CreateValidationResult<TResponse>(validationProblems);

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(ValidationProblem[] validationProblems)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (ValidationResult.WithErrors(validationProblems) as TResult)!;

        var validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object[] { validationProblems });

        return (validationResult as TResult)!;
    }
}