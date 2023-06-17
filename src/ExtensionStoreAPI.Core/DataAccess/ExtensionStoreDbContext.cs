using System.Reflection;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExtensionStoreAPI.Core.DataAccess;

public class ExtensionStoreDbContext : DbContext
{
    public DbSet<ExtensionEntity> ExtensionEntities => Set<ExtensionEntity>();
    public DbSet<ExtensionFileEntity> ExtensionFileEntities => Set<ExtensionFileEntity>();
    public DbSet<ExtensionDownloadInfoEntity> ExtensionDownloadInfoEntities => Set<ExtensionDownloadInfoEntity>();

    public ExtensionStoreDbContext(DbContextOptions<ExtensionStoreDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var applyGenericMethod =
            typeof(ModelBuilder).GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.Public);
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(c => c is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false })) 
        {
            foreach (var i in type.GetInterfaces())
            {
                if (!i.IsConstructedGenericType 
                    || i.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>)) continue;
                
                var applyConcreteMethod = applyGenericMethod?.MakeGenericMethod(i.GenericTypeArguments[0]);
                applyConcreteMethod?.Invoke(modelBuilder, new []
                {
                    Activator.CreateInstance(type)
                });
                break;
            }
        }
    }
    
}