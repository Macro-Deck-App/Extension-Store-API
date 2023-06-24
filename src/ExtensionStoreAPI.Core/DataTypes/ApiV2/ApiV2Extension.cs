using ExtensionStoreAPI.Core.Enums;

namespace ExtensionStoreAPI.Core.DataTypes.ApiV2;

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
    public IList<ApiV2ExtensionFile> ExtensionFiles { get; set; }

    public ApiV2Extension(string packageId,
        ExtensionType extensionType,
        string name,
        string category,
        string author,
        string description,
        string gitHubRepository,
        string dSupportUserId,
        IList<ApiV2ExtensionFile> extensionFiles)
    {
        PackageId = packageId;
        ExtensionType = extensionType;
        Name = name;
        Category = category;
        Author = author;
        Description = description;
        GitHubRepository = gitHubRepository;
        DSupportUserId = dSupportUserId;
        ExtensionFiles = extensionFiles;
    }
}