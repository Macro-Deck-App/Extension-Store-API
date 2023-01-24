using MacroDeckExtensionStoreLibrary.DataAccess.Entities;

namespace MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;

public interface IExtensionFileRepository
{
    public Task<bool> ExistAsync(string packageId, string version);
    public Task<ExtensionFileEntity[]> GetFilesAsync(string packageId);
    public Task<ExtensionFileEntity?> GetFileAsync(string packageId, int? targetApiVersion = null, string version = "latest");
    public Task CreateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity);
    public Task DeleteFileAsync(string packageId, string version);
    public Task UpdateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity);
}