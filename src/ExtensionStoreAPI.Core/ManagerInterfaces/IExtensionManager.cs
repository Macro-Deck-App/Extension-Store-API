using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.ManagerInterfaces;

public interface IExtensionManager
{
    public ValueTask<Extension> GetByPackageIdAsync(string packageId);
    public ValueTask<ExtensionSummary> GetSummaryByPackageIdAsync(string packageId);
    public ValueTask<string[]> GetCategoriesAsync(Filter filter);
    public ValueTask<List<ExtensionSummary>> GetTopDownloadsOfMonth(Filter filter, int month, int year, int count);
    public ValueTask<bool> ExistsAsync(string packageId);
    public ValueTask<PagedList<ExtensionSummary>> GetAllAsync(string? searchString, Filter? filter, Pagination pagination);
    public ValueTask CreateAsync(Extension extension);
    public ValueTask<FileStream> GetIconStreamAsync(string packageId);
}