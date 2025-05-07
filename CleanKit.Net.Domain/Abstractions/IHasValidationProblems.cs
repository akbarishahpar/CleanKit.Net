using CleanKit.Net.Domain.Primitives;

namespace CleanKit.Net.Domain.Abstractions;

public interface IHasValidationProblems
{
    public ValidationProblem[] ValidationProblems { get; set; }
}