using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;

public interface IExtensionRepository
{
    public ValueTask<bool> ExistAsync(string packageId);
    public ValueTask<string[]> GetCategoriesAsync(Filter filter);
    public ValueTask<ExtensionEntity?> GetByPackageIdAsync(string packageId);
    public ValueTask<PagedList<ExtensionEntity>> GetAllAsync(string? searchString, Filter? filter, Pagination pagination);
    public ValueTask<ExtensionEntity> CreateExtensionAsync(ExtensionEntity extensionEntity);
    public ValueTask DeleteExtensionAsync(string packageId);
    public ValueTask<ExtensionEntity> UpdateExtensionAsync(ExtensionEntity extensionEntity);
    public ValueTask UpdateDescription(string packageId, string description);
    public Task UpdateAuthorDiscordUserId(string extensionManifestPackageId, ulong? extensionManifestAuthorDiscordUserId);
}