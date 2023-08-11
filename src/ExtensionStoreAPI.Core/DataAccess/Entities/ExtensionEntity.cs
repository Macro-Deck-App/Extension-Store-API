using ExtensionStoreAPI.Core.Enums;

namespace ExtensionStoreAPI.Core.DataAccess.Entities;

public class ExtensionEntity : BaseCreatedUpdatedEntity
{
    public string PackageId { get; set; } = string.Empty;
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string Author { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string GitHubRepository { get; set; } = string.Empty;
    public string? DSupportUserId { get; set; }
    public ICollection<ExtensionFileEntity>? ExtensionFiles { get; set; }
    public ICollection<ExtensionDownloadInfoEntity>? Downloads { get; set; }
}