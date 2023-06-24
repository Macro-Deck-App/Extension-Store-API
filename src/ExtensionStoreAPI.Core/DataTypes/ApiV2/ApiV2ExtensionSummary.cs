using ExtensionStoreAPI.Core.Enums;

namespace ExtensionStoreAPI.Core.DataTypes.ApiV2;

/// <summary>
/// DO NOT CHANGE ANYTHING BECAUSE OF COMPATIBILITY REASONS
/// </summary>

public class ApiV2ExtensionSummary
{
    public string PackageId { get; set; }
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string GitHubRepository { get; set; }
    public string DSupportUserId { get; set; }

    public ApiV2ExtensionSummary(string packageId,
        ExtensionType extensionType,
        string name,
        string author,
        string description,
        string gitHubRepository,
        string dSupportUserId)
    {
        PackageId = packageId;
        ExtensionType = extensionType;
        Name = name;
        Author = author;
        Description = description;
        GitHubRepository = gitHubRepository;
        DSupportUserId = dSupportUserId;
    }
}