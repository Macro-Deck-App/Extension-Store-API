using ExtensionStoreAPI.Core.Enums;

#pragma warning disable CS8618

namespace ExtensionStoreAPI.Core.Models;

public class Extension
{
    public string PackageId { get; set; }
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string GitHubRepository { get; set; }
    public string DSupportUserId { get; set; }
    public long TotalDownloads { get; set; }
    
    public IList<ExtensionFile> ExtensionFiles { get; set; }
}