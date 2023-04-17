using MacroDeckExtensionStoreLibrary.Enums;
#pragma warning disable CS8618

namespace MacroDeckExtensionStoreLibrary.DataAccess.Entities;

public class ExtensionEntity
{
    public long ExtensionId { get; set; }
    public string PackageId { get; set; }
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; }
    public string? Category { get; set; }
    public string Author { get; set; }
    public string? Description { get; set; }
    public string GitHubRepository { get; set; }
    public string? DSupportUserId { get; set; }
    public ICollection<ExtensionFileEntity> ExtensionFiles { get; set; }
    public ICollection<ExtensionDownloadInfoEntity> Downloads { get; set; }
}