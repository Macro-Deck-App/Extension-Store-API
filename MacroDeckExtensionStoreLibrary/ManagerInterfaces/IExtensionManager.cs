using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreLibrary.ManagerInterfaces;

public interface IExtensionManager
{
    public Task<ExtensionSummary[]> GetExtensionsAsync();
    public Task<Extension?> GetByPackageIdAsync(string packageId);
    public Task<ExtensionSummary[]> SearchAsync(string query);
    public Task CreateAsync(Extension extension);
    public Task DeleteAsync(string packageId);
}