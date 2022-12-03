using MacroDeckExtensionStoreLibrary.Data;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.Repositories;

public class ExtensionsDbRepository : IExtensionsRepository
{
    private readonly ExtensionStoreDbContext _dbContext;

    public ExtensionsDbRepository(ExtensionStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Extension[]> GetExtensionsAsync()
    {
        var extensions = await _dbContext.Extensions.ToArrayAsync();
        return extensions;
    }

    public async Task<Extension?> GetExtensionByPackageIdAsync(string packageId)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == packageId);
        return existingExtension;
    }

    public async Task AddExtensionAsync(Extension extension)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == extension.PackageId);
        if (existingExtension != null)
        {
            await UpdateExtensionAsync(extension);
            return;
        }
        await _dbContext.Extensions.AddAsync(extension);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteExtensionAsync(string packageId)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (existingExtension == null)
        {
            return;
        }
        _dbContext.Extensions.Remove(existingExtension);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateExtensionAsync(Extension extension)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == extension.PackageId);
        if (existingExtension == null)
        {
            throw new KeyNotFoundException();
        }
        extension.ExtensionId = existingExtension.ExtensionId;
        _dbContext.Entry(existingExtension).CurrentValues.SetValues(extension);
        await _dbContext.SaveChangesAsync();
    }
}