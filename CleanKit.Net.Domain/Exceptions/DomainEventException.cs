using CleanKit.Net.Domain.Primitives.Error;

namespace CleanKit.Net.Domain.Exceptions;

public class DomainEventException : Exception
{
    public DomainEventException(Error error) : base(error.ToString())
    {
    }
}