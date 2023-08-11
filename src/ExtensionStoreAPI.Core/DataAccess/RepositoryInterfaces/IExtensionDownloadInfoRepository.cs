using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.Request;

namespace ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;

public interface IExtensionDownloadInfoRepository
{
    public ValueTask IncreaseDownloadCounter(string packageId, string version);
    public ValueTask<long> GetDownloadsAsync(string packageId, DateOnly? startDate = null, DateOnly? endDate = null);
    public ValueTask<List<ExtensionEntity>> GetTopDownloadsOfMonth(Filter? filter, int month, int year, int count);
}