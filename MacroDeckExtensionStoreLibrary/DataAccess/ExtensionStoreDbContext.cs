using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.DataAccess;

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
        modelBuilder.ApplyConfiguration(new ExtensionEntityConfig());
        modelBuilder.ApplyConfiguration(new ExtensionFileEntityConfig());
        modelBuilder.ApplyConfiguration(new ExtensionDownloadInfoEntityConfig());
    }
    
}