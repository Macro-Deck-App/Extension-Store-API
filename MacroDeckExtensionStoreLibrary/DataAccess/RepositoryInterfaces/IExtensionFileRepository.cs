using MacroDeckExtensionStoreLibrary.DataAccess.Entities;

namespace MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;

public interface IExtensionFileRepository
{
    public Task<bool> ExistAsync(string packageId, string version);
    public Task<ExtensionFileEntity[]> GetFiles(string packageId);
    public Task<ExtensionFileEntity?> GetFile(string packageId, int targetApiVersion, string version);
    public Task CreateFile(string packageId, ExtensionFileEntity extensionFileEntity);
    public Task DeleteFile(string packageId, string version);
    public Task UpdateFile(string packageId, ExtensionFileEntity extensionFileEntity);
}