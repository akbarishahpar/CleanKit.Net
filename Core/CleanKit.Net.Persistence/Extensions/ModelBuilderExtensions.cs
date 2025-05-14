using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pluralize.NET;
using System.Reflection;

namespace CleanKit.Net.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Singularization table name like Posts to Post or People to Person
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddSingularizationTableNameConvention(this ModelBuilder modelBuilder)
    {
        var pluralizer = new Pluralizer();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            entityType.SetTableName(pluralizer.Singularize(tableName));
        }
    }

    /// <summary>
    /// Pluralizing table name like Post to Posts or Person to People
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddPluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        var pluralizer = new Pluralizer();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            entityType.SetTableName(pluralizer.Pluralize(tableName));
        }
    }

    /// <summary>
    /// Set NEWSEQUENTIALID() sql function for all columns named "Id"
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddSequentialGuidForIdConvention(this ModelBuilder modelBuilder)
    {
        modelBuilder.AddDefaultValueSqlConvention("Id", typeof(Guid), "NEWSEQUENTIALID()");
    }

    /// <summary>
    /// Set DefaultValueSql for specific property name and type
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="propertyName">Name of property wants to set DefaultValueSql for</param>
    /// <param name="propertyType">Type of property wants to set DefaultValueSql for </param>
    /// <param name="defaultValueSql">DefaultValueSql like "NEWSEQUENTIALID()"</param>
    public static void AddDefaultValueSqlConvention(
        this ModelBuilder modelBuilder,
        string propertyName,
        Type propertyType,
        string defaultValueSql
    )
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var property = entityType
                .GetProperties()
                .SingleOrDefault(
                    p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)
                );
            if (property != null && property.ClrType == propertyType)
                property.SetDefaultValueSql(defaultValueSql);
        }
    }

    /// <summary>
    /// Set DeleteBehavior.Restrict by default for relations
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
    {
        IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
        foreach (IMutableForeignKey fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }

    /// <summary>
    /// Dynamically load all IEntityTypeConfiguration with Reflection
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterEntityTypeConfiguration(
        this ModelBuilder modelBuilder,
        params Assembly[] assemblies
    )
    {
        var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods()
            .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

        var types = assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

        foreach (var type in types)
        {
            foreach (var i in type.GetInterfaces())
            {
                if (
                    i.IsConstructedGenericType
                    && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                )
                {
                    var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(
                        i.GenericTypeArguments[0]
                    );
                    var instance = Activator.CreateInstance(type);
                    if (instance is not null)
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { instance });
                }
            }
        }
    }

    /// <summary>
    /// Dynamically register all Entities that inherit from specific BaseType
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterAllEntities<TBaseType>(
        this ModelBuilder modelBuilder,
        params Assembly[] assemblies
    )
    {
        var types = assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(
                c =>
                    c.IsClass
                    && !c.IsAbstract
                    && c.IsPublic
                    && typeof(TBaseType).IsAssignableFrom(c)
            );

        foreach (Type type in types)
            modelBuilder.Entity(type);
    }
}
