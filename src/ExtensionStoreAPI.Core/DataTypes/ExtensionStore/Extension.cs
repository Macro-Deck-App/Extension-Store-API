using ExtensionStoreAPI.Core.Enums;

namespace ExtensionStoreAPI.Core.DataTypes.ExtensionStore;

public class Extension
{
    public string? PackageId { get; set; }
    public ExtensionType? ExtensionType { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? GitHubRepository { get; set; }
    public ulong? DSupportUserId { get; set; }
    public IList<ExtensionFile>? ExtensionFiles { get; set; }
}