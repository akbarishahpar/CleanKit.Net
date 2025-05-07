namespace CleanKit.Net.Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
