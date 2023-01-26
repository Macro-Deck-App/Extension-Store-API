using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;

public interface IExtensionRepository
{
    public Task<bool> ExistAsync(string packageId);
    public Task<ExtensionEntity[]> GetExtensionsAsync(Filter filter, Pagination pagination);
    public Task<ExtensionEntity?> GetByPackageIdAsync(string packageId);
    public Task<ExtensionEntity[]> SearchAsync(string query);
    public Task CreateExtensionAsync(ExtensionEntity extensionEntity);
    public Task DeleteExtensionAsync(string packageId);
    public Task UpdateExtensionAsync(ExtensionEntity extensionEntity);
    public Task UpdateDescription(string packageId, string description);
    public Task CountDownloadAsync(string packageId, string version);
    public Task<long> GetDownloadCountAsync(string packageId);
    public Task<ExtensionDownloadInfoEntity[]> GetDownloadsAsync(string packageId, DateOnly? startDate = null, DateOnly? endDate = null);
}