using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.Enums;
using ExtensionStoreAPI.Core.ErrorHandling;
using ExtensionStoreAPI.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExtensionStoreAPI.Core.DataAccess.Repositories;

public class ExtensionRepository : IExtensionRepository
{
    private readonly ExtensionStoreDbContext _context;

    public ExtensionRepository(ExtensionStoreDbContext context)
    {
        _context = context;
    }

    public async ValueTask<bool> ExistAsync(string packageId)
    {
        return await _context.GetNoTrackingSet<ExtensionEntity>()
            .AnyAsync(x => x.PackageId == packageId);
    }

    public async ValueTask<string[]> GetCategoriesAsync(Filter filter)
    {
        return await _context.GetNoTrackingSet<ExtensionEntity>()
            .Where(x =>
                x.Category != null &&
                x.ExtensionType == ExtensionType.Plugin && filter.ShowPlugins
                || x.ExtensionType == ExtensionType.IconPack && filter.ShowIconPacks)
            .Select(x => x.Category!)
            .Distinct()
            .ToArrayAsync();
    }

    public async ValueTask<ExtensionEntity?> GetByPackageIdAsync(string packageId)
    {
        return await _context.GetNoTrackingSet<ExtensionEntity>()
            .Include(x => x.ExtensionFiles)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
    }

    public async ValueTask<PagedList<ExtensionEntity>> GetAllAsync(
        string? searchString,
        Filter? filter,
        Pagination pagination)
    {
        var query = _context.GetNoTrackingSet<ExtensionEntity>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.ToLower().Trim();

            query = query.Where(x =>
                x.PackageId.ToLower().Contains(searchString)
                || x.Name.ToLower().Contains(searchString)
                || x.Author.ToLower().Contains(searchString));
        }

        if (filter is not null)
        {
            query = query.Where(
                x => (filter.Category == null || x.Category == filter.Category) &&
                     (filter.ShowPlugins && x.ExtensionType == ExtensionType.Plugin
                      || filter.ShowIconPacks && x.ExtensionType == ExtensionType.IconPack));
        }

        query = query.OrderByDescending(x => x.CreatedTimestamp);

        return await query.ToPagedListAsync(pagination.Page, pagination.PageSize);
    }

    public async ValueTask<ExtensionEntity> CreateExtensionAsync(ExtensionEntity extensionEntity)
    {
        var exists = await _context.GetNoTrackingSet<ExtensionEntity>()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        
        if (exists)
        {
            throw new ErrorCodeException(ErrorCodes.PackageIdNotFound);
        }

        return await _context.CreateAsync(extensionEntity);
    }

    public async ValueTask DeleteExtensionAsync(string packageId)
    {
        var extensionEntity = await _context.GetNoTrackingSet<ExtensionEntity>()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        
        if (extensionEntity is null)
        {
            throw new ErrorCodeException(ErrorCodes.PackageIdNotFound);
        }

        await _context.DeleteAsync<ExtensionEntity>(extensionEntity.Id);
    }

    public async ValueTask<ExtensionEntity> UpdateExtensionAsync(ExtensionEntity extensionEntity)
    {
        var exists = await _context.GetNoTrackingSet<ExtensionEntity>()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        
        if (!exists)
        {
            throw new ErrorCodeException(ErrorCodes.PackageIdNotFound);
        }

        return await _context.UpdateAsync(extensionEntity);
    }

    public async ValueTask UpdateDescription(string packageId, string description)
    {
        var extensionEntity = await _context.Set<ExtensionEntity>()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        
        if (extensionEntity is null)
        {
            throw new ErrorCodeException(ErrorCodes.PackageIdNotFound);
        }

        extensionEntity.Description = description;
        
        await _context.UpdateAsync(extensionEntity);
    }

    public async Task UpdateAuthorDiscordUserId(string packageId, ulong? extensionManifestAuthorDiscordUserId)
    {
        if (!extensionManifestAuthorDiscordUserId.HasValue)
        {
            return;
        }
        
        await _context.Set<ExtensionEntity>()
            .Where(x => x.PackageId == packageId)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.DSupportUserId, extensionManifestAuthorDiscordUserId.Value));
    }
}