using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.DataAccess.Repositories;

public class ExtensionRepository : IExtensionRepository
{
    private readonly ExtensionStoreDbContext _context;

    public ExtensionRepository(ExtensionStoreDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public async Task<bool> ExistAsync(string packageId)
    {
        var exist = await _context.ExtensionEntities.AsNoTracking().AnyAsync(x => x.PackageId == packageId);
        return exist;
    }

    public async Task<ExtensionEntity[]> GetExtensionsAsync()
    {
        var extensionEntities = await _context.ExtensionEntities.AsNoTracking().ToArrayAsync();
        return extensionEntities;
    }

    public async Task<ExtensionEntity?> GetByPackageIdAsync(string packageId)
    {
        var extensionEntity = await _context.ExtensionEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        return extensionEntity;
    }

    public async Task<ExtensionEntity[]> SearchAsync(string query)
    {
        query = query.ToLower().Trim();
        var matches = await _context.ExtensionEntities.AsNoTracking().Where(x => 
                                                            x.PackageId.ToLower().Contains(query) ||
                                                            x.Name.ToLower().Contains(query) ||
                                                            x.Author.ToLower().Contains(query) ||
                                                            x.DSupportUserId.ToLower().Contains(query)).ToArrayAsync();
        return matches;
    }

    public async Task CreateExtensionAsync(ExtensionEntity extensionEntity)
    {
        var exists = await _context.ExtensionEntities.AsNoTracking()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        if (exists)
        {
            await UpdateExtensionAsync(extensionEntity);
            return;
        }

        await _context.ExtensionEntities.AddAsync(extensionEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExtensionAsync(string packageId)
    {
        var extensionEntity = await _context.ExtensionEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            return;
        }

        _context.ExtensionEntities.Remove(extensionEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExtensionAsync(ExtensionEntity extensionEntity)
    {
        var exists = await _context.ExtensionEntities.AsNoTracking()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        if (!exists)
        {
            await CreateExtensionAsync(extensionEntity);
            return;
        }
        
        _context.Entry(extensionEntity).CurrentValues.SetValues(extensionEntity);
        await _context.SaveChangesAsync();
    }

    public async Task CountDownloadAsync(string packageId)
    {
        var extensionEntity = await _context.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity != null)
        {
            extensionEntity.Downloads += 1;
            await _context.SaveChangesAsync();
        }
    }
}