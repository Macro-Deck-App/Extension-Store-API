using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.ManagerInterfaces;

public interface IExtensionFileManager
{
    public Task<bool> ExistAsync(string packageId, string version);
    public Task<ExtensionFile[]> GetFilesAsync(string packageId);
    public Task<ExtensionFile> GetFileAsync(string packageId, int? targetApiVersion = null, string version = "latest");
    public Task<ExtensionFileUploadResult> CreateFileAsync(Stream packageStream);
    public Task<FileStream> GetFileStreamAsync(string packageId, int targetApiVersion, string version);
}