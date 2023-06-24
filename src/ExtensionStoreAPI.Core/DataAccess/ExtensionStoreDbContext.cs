using System.Reflection;
using ExtensionStoreAPI.Core.Configuration;
using ExtensionStoreAPI.Core.DataAccess.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ExtensionStoreAPI.Core.DataAccess;

public class ExtensionStoreDbContext : DbContext
{
    public ExtensionStoreDbContext(DbContextOptions<ExtensionStoreDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var loggerFactory = new LoggerFactory()
            .AddSerilog();
        var connectionString = ExtensionStoreApiConfig.DatabaseConnectionStringOverride
                               ?? ExtensionStoreApiConfig.DatabaseConnectionString;
        options.UseNpgsql(connectionString);
        options.UseLoggerFactory(loggerFactory);
        options.AddInterceptors(new SaveChangesInterceptor());
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("extensionstore");
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