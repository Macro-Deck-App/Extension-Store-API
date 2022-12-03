using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.Interfaces;

public interface IExtensionsRepository
{
    public Task<Extension[]> GetExtensionsAsync();
    public Task<Extension?> GetExtensionByPackageIdAsync(string packageId);
    public Task AddExtensionAsync(Extension extension);
    public Task DeleteExtensionAsync(string packageId);
    public Task UpdateExtensionAsync(Extension extension);
}