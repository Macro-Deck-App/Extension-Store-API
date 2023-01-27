using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using MacroDeckExtensionStoreLibrary.Enums;
using MacroDeckExtensionStoreLibrary.Exceptions;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MacroDeckExtensionStoreLibrary.DataAccess.Repositories;

public class ExtensionRepository : IExtensionRepository
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ExtensionRepository(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<bool> ExistAsync(string packageId)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var exist = await context.ExtensionEntities.AsNoTracking()
            .AnyAsync(x => x.PackageId == packageId);
        return exist;
    }

    public async Task<PagedData<ExtensionEntity[]>> GetExtensionsPagedAsync(Filter filter, Pagination pagination)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var filteredExtensionEntities = context.ExtensionEntities.AsNoTracking().Include(x => x.Downloads).Where(
                x =>
                    filter.ShowPlugins && x.ExtensionType == ExtensionType.Plugin ||
                    filter.ShowIconPacks && x.ExtensionType == ExtensionType.IconPack)
            .OrderBy(x => x.Name);
        var extensionEntitiesCount = await filteredExtensionEntities.CountAsync();
        var pagedExtensionEntities = 
            await filteredExtensionEntities.Skip(pagination.Page * pagination.ItemsPerPage)
            .Take(pagination.ItemsPerPage).ToArrayAsync();

        var pagedData = new PagedData<ExtensionEntity[]>
        {
            TotalItemsCount = extensionEntitiesCount,
            CurrentPage = pagination.Page,
            ItemsPerPage = pagination.ItemsPerPage,
            Data = pagedExtensionEntities
        };

        return pagedData;
    }

    public async Task<ExtensionEntity[]> GetTopDownloadsOfMonth(Filter filter, int month, int year, int count)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionEntities = await context.ExtensionEntities.AsNoTracking().Include(x => x.Downloads)
            .Where(x => filter.ShowPlugins && x.ExtensionType == ExtensionType.Plugin 
                        || filter.ShowIconPacks && x.ExtensionType == ExtensionType.IconPack)
            .OrderByDescending(d =>
                d.Downloads.Count(y =>
                    y.DownloadDateTime.Year == year && y.DownloadDateTime.Month == month)).Take(count)
            .ToArrayAsync();

        return extensionEntities;
    }

    public async Task<ExtensionEntity?> GetByPackageIdAsync(string packageId)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionEntity = await context.ExtensionEntities.AsNoTracking()
            .Include(x => x.ExtensionFiles)
            .Include(x => x.Downloads)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        return extensionEntity;
    }

    public async Task<ExtensionEntity[]> SearchAsync(string query)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        query = query.ToLower().Trim();
        var matches = await context.ExtensionEntities.AsNoTracking().Include(x => x.Downloads).Where(x =>
            x.PackageId.ToLower().Contains(query) ||
            x.Name.ToLower().Contains(query) ||
            x.Author.ToLower().Contains(query) ||
            (x.DSupportUserId != null && x.DSupportUserId.ToLower().Contains(query)))
            .OrderBy(x => x.Name)
            .Take(25)
            .ToArrayAsync();
        return matches;
    }

    public async Task CreateExtensionAsync(ExtensionEntity extensionEntity)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var exists = await context.ExtensionEntities.AsNoTracking()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        if (exists)
        {
            await UpdateExtensionAsync(extensionEntity);
            return;
        }

        await context.ExtensionEntities.AddAsync(extensionEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteExtensionAsync(string packageId)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionEntity = await context.ExtensionEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }

        context.ExtensionEntities.Remove(extensionEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateExtensionAsync(ExtensionEntity extensionEntity)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var exists = await context.ExtensionEntities.AsNoTracking()
            .AnyAsync(x => x.PackageId == extensionEntity.PackageId);
        if (!exists)
        {
            await CreateExtensionAsync(extensionEntity);
            return;
        }
        
        context.Entry(extensionEntity).CurrentValues.SetValues(extensionEntity);
        await context.SaveChangesAsync();
    }

    public async Task CountDownloadAsync(string packageId, string version)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionEntity = await context.ExtensionEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            return;
        }
        var extensionDownloadInfoEntity = new ExtensionDownloadInfoEntity
        {
            ExtensionId = extensionEntity.ExtensionId,
            DownloadedVersion = version,
            DownloadDateTime = DateTime.Now
        };
        await context.ExtensionDownloadInfoEntities.AddAsync(extensionDownloadInfoEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateDescription(string packageId, string description)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var extensionEntity = await context.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }

        extensionEntity.Description = description;
        await context.SaveChangesAsync();
    }
    
    public async Task<long> GetDownloadCountAsync(string packageId)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var count = await context.ExtensionDownloadInfoEntities.AsNoTracking().Include(x => x.ExtensionEntity)
            .CountAsync(x => x.ExtensionEntity.PackageId == packageId);
        return count;
    }

    public async Task<ExtensionDownloadInfoEntity[]> GetDownloadsAsync(string packageId, DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        var startDateTime = startDate?.ToDateTime(default);
        var endDateTime = endDate?.ToDateTime(default);
        var downloads = await context.ExtensionDownloadInfoEntities.AsNoTracking().Include(x => x.ExtensionEntity)
            .Where(x => (!startDate.HasValue || x.DownloadDateTime >= startDateTime) 
                        && (!endDateTime.HasValue || x.DownloadDateTime <= endDateTime)).ToArrayAsync();
        
        return downloads;
    }
}