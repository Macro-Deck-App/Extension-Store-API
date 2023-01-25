using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.ManagerInterfaces;

public interface IExtensionManager
{
    public Task<ExtensionSummary[]> GetExtensionsAsync();
    public Task<Extension?> GetByPackageIdAsync(string packageId);
    public Task<bool> ExistsAsync(string packageId);
    public Task<ExtensionSummary[]> SearchAsync(string query);
    public Task CreateAsync(Extension extension);
    public Task DeleteAsync(string packageId);
    public Task<FileStream?> GetIconStreamAsync(string packageId);
    public Task CountDownloadAsync(string packageId, string version);
    public Task<long> GetDownloadCountAsync(string packageId);
    public Task<ExtensionDownloadInfo[]> GetDownloadsAsync(string packageId, DateOnly? startDate = null,
        DateOnly? endDate = null);
}