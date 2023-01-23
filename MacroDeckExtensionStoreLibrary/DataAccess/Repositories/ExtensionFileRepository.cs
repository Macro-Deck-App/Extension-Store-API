using JetBrains.Annotations;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.DataAccess.Repositories;

[UsedImplicitly]
public class ExtensionFileRepository : IExtensionFileRepository
{
    private readonly ExtensionStoreDbContext _context;

    public ExtensionFileRepository(ExtensionStoreDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistAsync(string packageId, string version)
    {
        var exist = await _context.ExtensionFileEntities.AsNoTracking().Include(x => x.ExtensionEntity)
            .AnyAsync(x => x.Version == version);
        return exist;
    }

    public async Task<ExtensionFileEntity[]> GetFiles(string packageId)
    {
        var extensionEntity =
            await _context.ExtensionEntities.AsNoTracking()
                .Include(x => x.ExtensionFiles)
                .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            return Array.Empty<ExtensionFileEntity>();
        }

        var extensionFileEntities = extensionEntity.ExtensionFiles.ToArray();
        return extensionFileEntities;
    }

    public async Task<ExtensionFileEntity?> GetFile(string packageId, int targetApiVersion, string version = "latest")
    {
        if (version.ToLower() == "latest")
        {
            var latestExtensionFileEntity = await _context.ExtensionFileEntities.Include(x => x.ExtensionEntity).AsNoTracking()
                .Where(x => x.ExtensionEntity.PackageId == packageId &&
                            x.MinApiVersion <= targetApiVersion)
                .OrderBy(x => x.UploadDateTime)
                .FirstAsync();
            return latestExtensionFileEntity;
        }

        var extensionFileEntity = await _context.ExtensionFileEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExtensionEntity.PackageId == packageId &&
                                      x.MinApiVersion <= targetApiVersion && x.Version == version);
        
        return extensionFileEntity;
    }

    public async Task CreateFile(string packageId, ExtensionFileEntity extensionFileEntity)
    {
        var exists = await _context.ExtensionFileEntities.AsNoTracking().Include(x => x.ExtensionEntity).AnyAsync(x =>
            x.ExtensionEntity.PackageId == packageId && x.Version == extensionFileEntity.Version);
        if (exists)
        {
            await UpdateFile(packageId, extensionFileEntity);
            return;
        }

        await _context.ExtensionFileEntities.AddAsync(extensionFileEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFile(string packageId, string version)
    {
        var extensionFileEntity = await _context.ExtensionFileEntities.AsNoTracking()
            .Include(x => x.ExtensionEntity)
            .FirstOrDefaultAsync(x =>
            x.ExtensionEntity.PackageId == packageId && x.Version == version);
        if (extensionFileEntity == null)
        {
            return;
        }

        _context.ExtensionFileEntities.Remove(extensionFileEntity);
        await _context.SaveChangesAsync();
    }

    public Task UpdateFile(string packageId, ExtensionFileEntity extensionFileEntity)
    {
        throw new NotImplementedException();
    }
}