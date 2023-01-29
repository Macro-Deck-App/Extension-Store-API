using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;

public interface IExtensionRepository
{
    public Task<bool> ExistAsync(string packageId);
    public Task<string?[]> GetCategoriesAsync(Filter filter);
    public Task<PagedData<ExtensionEntity[]>> GetExtensionsPagedAsync(Filter filter, Pagination pagination);
    public Task<ExtensionEntity[]> GetTopDownloadsOfMonth(Filter filter, int month, int year, int count);
    public Task<ExtensionEntity?> GetByPackageIdAsync(string packageId);
    public Task<PagedData<ExtensionEntity[]>> SearchAsync(string query, Filter filter, Pagination pagination);
    public Task CreateExtensionAsync(ExtensionEntity extensionEntity);
    public Task DeleteExtensionAsync(string packageId);
    public Task UpdateExtensionAsync(ExtensionEntity extensionEntity);
    public Task UpdateDescription(string packageId, string description);
    public Task CountDownloadAsync(string packageId, string version);
    public Task<long> GetDownloadCountAsync(string packageId);
    public Task<ExtensionDownloadInfoEntity[]> GetDownloadsAsync(string packageId, DateOnly? startDate = null, DateOnly? endDate = null);
}