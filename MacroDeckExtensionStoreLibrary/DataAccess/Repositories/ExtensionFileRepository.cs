using JetBrains.Annotations;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MacroDeckExtensionStoreLibrary.DataAccess.Repositories;

[UsedImplicitly]
public class ExtensionFileRepository : IExtensionFileRepository
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ExtensionFileRepository(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<bool> ExistAsync(string packageId, string version)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var exist = await context.ExtensionFileEntities.AsNoTracking().Include(x => x.ExtensionEntity)
            .AnyAsync(x => x.Version == version);
        return exist;
    }

    public async Task<ExtensionFileEntity[]> GetFilesAsync(string packageId)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionEntity =
            await context.ExtensionEntities.AsNoTracking()
                .Include(x => x.ExtensionFiles)
                .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            return Array.Empty<ExtensionFileEntity>();
        }

        var extensionFileEntities = extensionEntity.ExtensionFiles.ToArray();
        return extensionFileEntities;
    }

    public async Task<ExtensionFileEntity?> GetFileAsync(string packageId, int? targetApiVersion = null, string version = "latest")
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        if (version.ToLower() == "latest")
        {
            var latestExtensionFileEntity = await context.ExtensionFileEntities.Include(x => x.ExtensionEntity).AsNoTracking()
                .Where(x => x.ExtensionEntity.PackageId == packageId &&
                            (!targetApiVersion.HasValue || x.MinApiVersion <= targetApiVersion))
                .OrderBy(x => x.UploadDateTime)
                .FirstAsync();
            return latestExtensionFileEntity;
        }

        var extensionFileEntity = await context.ExtensionFileEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExtensionEntity.PackageId == packageId &&
                                      x.MinApiVersion <= targetApiVersion && x.Version == version);
        
        return extensionFileEntity;
    }

    public async Task CreateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var exists = await context.ExtensionFileEntities.AsNoTracking().Include(x => x.ExtensionEntity).AnyAsync(x =>
            x.ExtensionEntity.PackageId == packageId && x.Version == extensionFileEntity.Version);
        if (exists)
        {
            await UpdateFileAsync(packageId, extensionFileEntity);
            return;
        }

        await context.ExtensionFileEntities.AddAsync(extensionFileEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteFileAsync(string packageId, string version)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionFileEntity = await context.ExtensionFileEntities.AsNoTracking()
            .Include(x => x.ExtensionEntity)
            .FirstOrDefaultAsync(x =>
            x.ExtensionEntity.PackageId == packageId && x.Version == version);
        if (extensionFileEntity == null)
        {
            return;
        }

        context.ExtensionFileEntities.Remove(extensionFileEntity);
        await context.SaveChangesAsync();
    }

    public Task UpdateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity)
    {
        throw new NotImplementedException();
    }
}