using ExtensionStoreAPI.Core.DataAccess.Entities;

namespace ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;

public interface IExtensionFileRepository
{
    public ValueTask<bool> ExistAsync(string packageId, string version);
    public ValueTask<ExtensionFileEntity[]> GetFilesAsync(string packageId);
    public ValueTask<ExtensionFileEntity?> GetFileAsync(string packageId, int? targetApiVersion = null, string version = "latest");
    public ValueTask<ExtensionFileEntity> CreateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity);
    public ValueTask DeleteFileAsync(string packageId, string version);
}