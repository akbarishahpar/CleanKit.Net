using CleanKit.Net.Application.Abstractions.Data;
using CleanKit.Net.Domain.Abstractions;
using CleanKit.Net.Persistence.Abstractions;
using CleanKit.Net.Persistence.Extensions;
using CleanKit.Net.Persistence.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanKit.Net.Persistence.DatabaseContext;

public abstract class DatabaseContext : DbContext, IDatabaseContext, IUnitOfWork
{
    protected Assembly[]? EntitiesAssemblies;

    // ReSharper disable once PublicConstructorInAbstractClass
    public DatabaseContext()
    {
    }

    // ReSharper disable once PublicConstructorInAbstractClass
    public DatabaseContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Registering entities
        if (EntitiesAssemblies == null)
            throw new InvalidOperationException("EntitiesAssemblies is required to register entities");
        modelBuilder.RegisterEntityTypeConfiguration(EntitiesAssemblies);
        modelBuilder.RegisterAllEntities<IEntity>(EntitiesAssemblies);
        
        //Restricting delete behaviour
        modelBuilder.AddRestrictDeleteBehaviorConvention();

        //Making guids sequential
        modelBuilder.AddSequentialGuidForIdConvention();

        //Enabling pluralize for table names
        modelBuilder.AddPluralizingTableNameConvention();
    }

    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var dbContextTransaction = await Database.BeginTransactionAsync(cancellationToken);
        return new UnitOfWorkTransaction(dbContextTransaction);
    }
}