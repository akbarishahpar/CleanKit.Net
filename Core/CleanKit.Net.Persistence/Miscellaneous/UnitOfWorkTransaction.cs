using CleanKit.Net.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanKit.Net.Persistence.Miscellaneous;

public class UnitOfWorkTransaction : IUnitOfWorkTransaction
{
    private readonly IDbContextTransaction _dbContextTransaction;

    public UnitOfWorkTransaction(IDbContextTransaction dbContextTransaction)
        => _dbContextTransaction = dbContextTransaction;

    public void Dispose()
        => _dbContextTransaction.Dispose();


    public ValueTask DisposeAsync()
        => _dbContextTransaction.DisposeAsync();

    public Task CommitAsync(CancellationToken cancellationToken)
        => _dbContextTransaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken)
        => _dbContextTransaction.RollbackAsync(cancellationToken);
}