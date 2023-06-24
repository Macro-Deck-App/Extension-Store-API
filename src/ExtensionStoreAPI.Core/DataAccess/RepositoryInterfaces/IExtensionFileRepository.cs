using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;

public interface IExtensionFileRepository
{
    public ValueTask<bool> ExistAsync(string packageId, string version);
    public ValueTask<PagedList<ExtensionFileEntity>> GetFilesAsync(string packageId, Pagination pagination);
    public ValueTask<ExtensionFileEntity?> GetFileAsync(string packageId, string? version = null, int? targetApiVersion = null);
    public ValueTask<ExtensionFileEntity> CreateFileAsync(string packageId, ExtensionFileEntity extensionFileEntity);
    public ValueTask DeleteFileAsync(string packageId, string version);
}