using ExtensionStoreAPI.Core.Enums;

namespace ExtensionStoreAPI.Core.DataTypes.ExtensionStore;

public class ExtensionSummary
{
    public string PackageId { get; set; } = string.Empty;
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GitHubRepository { get; set; } = string.Empty;
    public string DSupportUserId { get; set; } = string.Empty;
    public long TotalDownloads { get; set; }

}