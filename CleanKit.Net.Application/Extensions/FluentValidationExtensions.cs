using FluentValidation;
using CleanKit.Net.Domain.Primitives;
using CleanKit.Net.Domain.Primitives.Error;

namespace CleanKit.Net.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        Error error
    )
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error), "The error is required");
        return rule.WithErrorCode(error.Code).WithMessage(error.Message);
    }
}
