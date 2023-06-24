using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.ManagerInterfaces;

public interface IExtensionManager
{
    public Task<Extension> GetByPackageIdAsync(string packageId);
    public Task<ExtensionSummary> GetSummaryByPackageIdAsync(string packageId);
    public Task<string[]> GetCategoriesAsync(Filter filter);
    public Task<List<ExtensionSummary>> GetTopDownloadsOfMonth(Filter filter, int month, int year, int count);
    public Task<bool> ExistsAsync(string packageId);
    public Task<PagedList<ExtensionSummary>> GetAllAsync(string? searchString, Filter? filter, Pagination pagination);
    public Task CreateAsync(Extension extension);
    public Task DeleteAllAsync();
    public Task DeleteAsync(string packageId);
    public Task<FileStream> GetIconStreamAsync(string packageId);
}