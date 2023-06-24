using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.Enums;
using ExtensionStoreAPI.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExtensionStoreAPI.Core.DataAccess.Repositories;

public class ExtensionDownloadInfoRepository : IExtensionDownloadInfoRepository
{
    private readonly ExtensionStoreDbContext _context;

    public ExtensionDownloadInfoRepository(ExtensionStoreDbContext context)
    {
        _context = context;
    }

    public async ValueTask IncreaseDownloadCounter(string packageId, string version)
    {
        var extension = await _context.Set<ExtensionEntity>()
            .Where(x => x.PackageId == packageId)
            .FirstOrDefaultAsync();

        if (extension == null)
        {
            return;
        }

        await _context.CreateAsync(new ExtensionDownloadInfoEntity
        {
            DownloadedVersion = version,
            ExtensionId = extension.Id
        });
    }

    public async ValueTask<long> GetDownloadsAsync(
        string packageId,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var startDateTime = startDate?.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate?.ToDateTime(TimeOnly.MaxValue);
        
        var query = _context.Set<ExtensionDownloadInfoEntity>()
            .Include(x => x.ExtensionEntity)
            .Where(x => x.ExtensionEntity != null && x.ExtensionEntity.PackageId == packageId);

        if (startDateTime.HasValue)
        {
            query = query.Where(x => x.CreatedTimestamp >= startDateTime);
        }

        if (endDateTime.HasValue)
        {
            query = query.Where(x => x.CreatedTimestamp <= endDateTime);
        }

        return await query.LongCountAsync();
    }

    public async ValueTask<List<ExtensionDownloadInfoEntity>> GetTopDownloadsOfMonth(
        Filter? filter,
        int month,
        int year,
        int count)
    {
        var query = _context.Set<ExtensionDownloadInfoEntity>()
            .Include(x => x.ExtensionEntity)
            .Where(x => x.CreatedTimestamp.Month == month && x.CreatedTimestamp.Year == year)
            .Where(x => x.ExtensionEntity != null)
            .OrderBy(x => x.ExtensionEntity!.Name)
            .Take(count);

        if (filter != null)
        {
            query = query.Where(x => (filter.ShowPlugins && x.ExtensionEntity!.ExtensionType == ExtensionType.Plugin)
                                     || (filter.ShowIconPacks 
                                         && x.ExtensionEntity!.ExtensionType == ExtensionType.IconPack));
        }

        return await query.ToListAsync();
    }
}