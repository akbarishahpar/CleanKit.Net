using CleanKit.Net.Domain.Abstractions;
using CleanKit.Net.Domain.Primitives.Error;

namespace CleanKit.Net.Domain.Primitives.Result;

public class ValidationResult : Result, IHasValidationProblems
{
    public static readonly ValidationError ValidationError = new(
        "ValidationError",
        "A validation problem occurred."
    );

    public ValidationProblem[] ValidationProblems { get; set; }

    private ValidationResult(ValidationProblem[] validationProblems) : base(false, ValidationError)
        => ValidationProblems = validationProblems;

    public static ValidationResult WithErrors(ValidationProblem[] errors) => new(errors);
}

public class ValidationResult<TValue> : Result<TValue?>, IHasValidationProblems
{
    public ValidationProblem[] ValidationProblems { get; set; }

    private ValidationResult(ValidationProblem[] validationProblems) : base(
        default,
        false,
        ValidationResult.ValidationError
    ) => ValidationProblems = validationProblems;

    public static ValidationResult<TValue> WithErrors(ValidationProblem[] errors) => new(errors);
}