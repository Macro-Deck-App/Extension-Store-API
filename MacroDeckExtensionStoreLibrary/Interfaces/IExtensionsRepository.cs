using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.Interfaces;

public interface IExtensionsRepository
{
    public Task<Extension[]> GetExtensionsAsync();
    public Task<Extension?> GetExtensionByPackageIdAsync(string packageId);
    public Task AddExtensionAsync(Extension extension);
    public Task DeleteExtensionAsync(string packageId);
    public Task UpdateExtensionAsync(Extension extension);
    public Task<ExtensionFile[]> GetExtensionFilesAsync(string packageId);
    public Task<ExtensionFile?> GetExtensionFileAsync(string packageId, int apiVersion, string version = "latest");
    public Task CountDownloadAsync(string packageId);
    public Task AddExtensionFileAsync(string packageId, ExtensionFile extensionFile);
}