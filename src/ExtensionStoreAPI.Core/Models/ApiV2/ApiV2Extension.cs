using ExtensionStoreAPI.Core.Enums;

// ReSharper disable CollectionNeverUpdated.Global
#pragma warning disable CS8618

namespace ExtensionStoreAPI.Core.Models.ApiV2;

/// <summary>
/// DO NOT CHANGE ANYTHING BECAUSE OF COMPATIBILITY REASONS
/// </summary>

public class ApiV2Extension
{
    public string PackageId { get; set; }
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string GitHubRepository { get; set; }
    public string DSupportUserId { get; set; }
    public long TotalDownloads { get; set; }
    public IList<ApiV2ExtensionFile> ExtensionFiles { get; set; }
}