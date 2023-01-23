using MacroDeckExtensionStoreLibrary.DataAccess.EntityConfigurations;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.DataAccess;

public class ExtensionStoreDbContext : DbContext
{
    public DbSet<Extension> Extensions => Set<Extension>();
    public DbSet<ExtensionFile> ExtensionFiles => Set<ExtensionFile>();

    public ExtensionStoreDbContext(DbContextOptions<ExtensionStoreDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExtensionEntityConfig());
        modelBuilder.ApplyConfiguration(new ExtensionFileEntityConfig());
    }
    
}