using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.Exceptions;
using ExtensionStoreAPI.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ExtensionStoreAPI.Core.DataAccess.Repositories;

[UsedImplicitly]
public class ExtensionFileRepository : IExtensionFileRepository
{
    private readonly ExtensionStoreDbContext _context;

    public ExtensionFileRepository(ExtensionStoreDbContext context)
    {
        _context = context;
    }

    public async ValueTask<bool> ExistAsync(string packageId, string version)
    {
        return await _context.GetNoTrackingSet<ExtensionFileEntity>()
            .Include(x => x.ExtensionEntity)
            .AnyAsync(x => x.ExtensionEntity != null
                           && x.ExtensionEntity.PackageId == packageId
                           && x.Version == version);
    }

    public async ValueTask<PagedList<ExtensionFileEntity>> GetFilesAsync(string packageId, Pagination pagination)
    {
        return await _context.GetNoTrackingSet<ExtensionFileEntity>()
            .Include(x => x.ExtensionEntity)
            .Where(x => x.ExtensionEntity != null && x.ExtensionEntity.PackageId == packageId)
            .ToPagedListAsync(pagination.Page, pagination.PageSize);
    }

    public async ValueTask<ExtensionFileEntity?> GetFileAsync(
        string packageId,
        string? version = null,
        int? targetApiVersion = null)
    {
        if (version is null)
        {
            return await _context.GetNoTrackingSet<ExtensionFileEntity>()
                .FilterTargetApiVersion(targetApiVersion)
                .OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync(x => x.ExtensionEntity != null && x.ExtensionEntity.PackageId == packageId);
        }

        return await _context.Set<ExtensionFileEntity>().Include(x => x.ExtensionEntity)
            .FilterTargetApiVersion(targetApiVersion)
            .Where(x => x.ExtensionEntity != null && x.ExtensionEntity.PackageId == packageId)
            .OrderByDescending(x => x.Version)
            .Take(1)
            .FirstOrDefaultAsync();
    }

    public async ValueTask<ExtensionFileEntity> CreateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity)
    {
        var exists = await _context.GetNoTrackingSet<ExtensionFileEntity>()
            .Include(x => x.ExtensionEntity)
            .AnyAsync(x => x.ExtensionEntity != null 
                           && x.ExtensionEntity.PackageId == packageId
                           && x.Version == extensionFileEntity.Version);
        
        if (exists)
        {
            throw ErrorCodeExceptions.VersionAlreadyExistsException();
        }

        var extensionEntity = await _context.Set<ExtensionEntity>()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }
        
        extensionFileEntity.ExtensionId = extensionEntity.Id;

        return await _context.CreateAsync(extensionFileEntity);
    }

    public async ValueTask DeleteFileAsync(string packageId, string version)
    {
        var extensionFileEntity = await _context.GetNoTrackingSet<ExtensionFileEntity>()
            .Include(x => x.ExtensionEntity)
            .FirstOrDefaultAsync(x => x.ExtensionEntity != null 
                                      && x.ExtensionEntity.PackageId == packageId
                                      && x.Version == version);
        
        if (extensionFileEntity == null)
        {
            return;
        }

        await _context.DeleteAsync<ExtensionFileEntity>(extensionFileEntity.Id);
    }
}