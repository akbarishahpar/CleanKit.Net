using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanKit.Net.Persistence.DatabaseContext;

public class DatabaseContextFactory<TDatabaseContext>
    : IDesignTimeDbContextFactory<TDatabaseContext>
    where TDatabaseContext : Persistence.DatabaseContext.DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContextFactory(string connectionString) => _connectionString = connectionString;

    public TDatabaseContext CreateDbContext(string[] args)
    {
        var dbContextBuilder = new DbContextOptionsBuilder<TDatabaseContext>();

        dbContextBuilder.UseSqlServer(_connectionString);

        var dbContext = Activator.CreateInstance(
            typeof(TDatabaseContext),
            dbContextBuilder.Options
        );

        if (dbContext == null)
            throw new Exception(
                $"Could not create an instance of database context with type '{typeof(TDatabaseContext).Name}'"
            );

        return (TDatabaseContext)dbContext;
    }
}
