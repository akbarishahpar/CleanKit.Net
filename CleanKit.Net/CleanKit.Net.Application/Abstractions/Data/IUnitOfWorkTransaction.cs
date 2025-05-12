namespace CleanKit.Net.Application.Abstractions.Data;

public interface IUnitOfWorkTransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}