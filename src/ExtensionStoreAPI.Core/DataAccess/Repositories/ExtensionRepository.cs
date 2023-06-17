using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.Enums;
using ExtensionStoreAPI.Core.Exceptions;
using ExtensionStoreAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtensionStoreAPI.Core.DataAccess.Repositories;

public class ExtensionRepository : IExtensionRepository
{
    private readonly ExtensionStoreDbContext _context;

    public ExtensionRepository(ExtensionStoreDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistAsync(string packageId)
    {
        return await _context.Set<ExtensionEntity>().AsNoTracking()
            .AnyAsync(x => x.PackageId == packageId);
    }

    public async Task<string[]> GetCategoriesAsync(Filter filter)
    {
        return await _context.Set<ExtensionEntity>().AsNoTracking()
            .Where(x =>
                x.Category != null &&
                x.ExtensionType == ExtensionType.Plugin && filter.ShowPlugins
                || x.ExtensionType == ExtensionType.IconPack && filter.ShowIconPacks)
            .Select(x => x.Category!)
            .Distinct()
            .ToArrayAsync();
    }

    public async Task<ExtensionEntity[]> GetAllExtensions()
    {
        return await _context.Set<ExtensionEntity>().AsNoTracking().ToArrayAsync();
    }

    public async Task<PagedData<ExtensionEntity[]>> GetExtensionsPagedAsync(Filter filter, Pagination pagination)
    {
        var filteredExtensionEntities = _context.Set<ExtensionEntity>().AsNoTracking()
            .Where(
                x => (filter.Category == null || x.Category == filter.Category) &&
                     (filter.ShowPlugins && x.ExtensionType == ExtensionType.Plugin ||
                      filter.ShowIconPacks && x.ExtensionType == ExtensionType.IconPack))
            .Include(x => x.Downloads)
            .OrderBy(x => x.Name);
        
        var extensionEntitiesCount = await filteredExtensionEntities.CountAsync();
        var offset = (pagination.Page - 1) * pagination.ItemsPerPage;
        var pagedExtensionEntities = 
            await filteredExtensionEntities.Skip(offset)
            .Take(pagination.ItemsPerPage).ToArrayAsync();

        return new PagedData<ExtensionEntity[]>
        {
            TotalItemsCount = extensionEntitiesCount,
            CurrentPage = pagination.Page,
            ItemsPerPage = pagination.ItemsPerPage,
            Data = pagedExtensionEntities
        };
    }

    public async Task<ExtensionEntity[]> GetTopDownloadsOfMonth(Filter filter, int month, int year, int count)
    {
        return await _context.Set<ExtensionEntity>().AsNoTracking()
            .Include(x => x.Downloads)
            .Where(x => filter.ShowPlugins && x.ExtensionType == ExtensionType.Plugin
                        || filter.ShowIconPacks && x.ExtensionType == ExtensionType.IconPack)
            .OrderByDescending(d =>
                d.Downloads.Count(y => y.CreatedTimestamp.Year == year && y.CreatedTimestamp.Month == month))
            .Take(count)
            .ToArrayAsync();
    }

    public async Task<ExtensionEntity?> GetByPackageIdAsync(string packageId)
    {
        return await _context.Set<ExtensionEntity>().AsNoTracking()
            .Include(x => x.ExtensionFiles)
            .Include(x => x.Downloads)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
    }

    public async Task<PagedData<ExtensionEntity[]>> SearchAsync(string query, Filter filter, Pagination pagination)
    {
        query = query.ToLower().Trim();
        var offset = (pagination.Page - 1) * pagination.ItemsPerPage;
        
        var results = await _context.Set<ExtensionEntity>().AsNoTracking().Include(x => x.Downloads)
            .Where(
                x => (filter.Category == null || x.Category == filter.Category) &&
                     (filter.ShowPlugins && x.ExtensionType == ExtensionType.Plugin ||
                      filter.ShowIconPacks && x.ExtensionType == ExtensionType.IconPack))
            .Where(x =>
                x.PackageId.ToLower().Contains(query) ||
                x.Name.ToLower().Contains(query) ||
                x.Author.ToLower().Contains(query) ||
                (x.DSupportUserId != null && x.DSupportUserId.ToLower().Contains(query)))
            .OrderBy(x => x.Name)
            .Skip(offset)
            .Take(pagination.ItemsPerPage)
            .ToArrayAsync();

        return new PagedData<ExtensionEntity[]>
        {
            TotalItemsCount = results.Length,
            CurrentPage = pagination.Page,
            ItemsPerPage = pagination.ItemsPerPage,
            Data = results
        };
    }

    public async Task CreateExtensionAsync(ExtensionEntity extensionEntity)
    {
        var exists = await _context.Set<ExtensionEntity>().AsNoTracking()
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
        var extensionEntity = await _context.Set<ExtensionEntity>().AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }

        _context.ExtensionEntities.Remove(extensionEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExtensionAsync(ExtensionEntity extensionEntity)
    {
        var exists = await _context.Set<ExtensionEntity>().AsNoTracking()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        
        if (!exists)
        {
            await CreateExtensionAsync(extensionEntity);
            return;
        }
        
        _context.Entry(extensionEntity).CurrentValues.SetValues(extensionEntity);
        await _context.SaveChangesAsync();
    }

    public async Task CountDownloadAsync(string packageId, string version)
    {
        var extensionEntity = await _context.ExtensionEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        
        if (extensionEntity == null)
        {
            return;
        }
        
        var extensionDownloadInfoEntity = new ExtensionDownloadInfoEntity
        {
            ExtensionId = extensionEntity.Id,
            DownloadedVersion = version
        };
        
        await _context.ExtensionDownloadInfoEntities.AddAsync(extensionDownloadInfoEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDescription(string packageId, string description)
    {
        var extensionEntity = await _context.Set<ExtensionEntity>()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }

        extensionEntity.Description = description;
        await _context.SaveChangesAsync();
    }
    
    public async Task<long> GetDownloadCountAsync(string packageId)
    {
        return await _context.Set<ExtensionDownloadInfoEntity>()
            .AsNoTracking()
            .Include(x => x.ExtensionEntity)
            .CountAsync(x => x.ExtensionEntity != null && x.ExtensionEntity.PackageId == packageId);
    }

    public async Task<ExtensionDownloadInfoEntity[]> GetDownloadsAsync(
        string packageId,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var startDateTime = startDate?.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate?.ToDateTime(TimeOnly.MaxValue);
        
        return await _context.Set<ExtensionDownloadInfoEntity>()
            .AsNoTracking()
            .Include(x => x.ExtensionEntity)
            .Where(x => (!startDate.HasValue || x.CreatedTimestamp >= startDateTime) 
                        && (!endDateTime.HasValue || x.CreatedTimestamp <= endDateTime))
            .ToArrayAsync();
    }
}