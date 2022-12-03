using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.Data;

public class ExtensionStoreDbContext : DbContext
{
    public DbSet<Extension> Extensions => Set<Extension>();
    public DbSet<ExtensionFile> ExtensionFiles => Set<ExtensionFile>();

    public ExtensionStoreDbContext(DbContextOptions<ExtensionStoreDbContext> options) : base(options)
    {
    }
    
}