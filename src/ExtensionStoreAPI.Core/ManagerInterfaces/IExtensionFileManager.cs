using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.ManagerInterfaces;

public interface IExtensionFileManager
{
    public Task<bool> ExistAsync(string packageId, string version);
    public Task<PagedList<ExtensionFile>> GetFilesAsync(string packageId, Pagination pagination);
    public Task<ExtensionFile> GetFileAsync(string packageId, string? version, int? targetApiVersion = null);
    public Task<ExtensionFileUploadResult> CreateFileAsync(Stream packageStream);
    public Task<Tuple<FileStream, string>> GetFileStreamAsync(string packageId, string? version, int targetApiVersion);
}