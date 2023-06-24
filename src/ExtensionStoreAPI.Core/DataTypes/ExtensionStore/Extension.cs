using ExtensionStoreAPI.Core.Enums;

namespace ExtensionStoreAPI.Core.DataTypes.ExtensionStore;

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
    public IList<ExtensionFile> ExtensionFiles { get; set; }

    public Extension(string packageId,
        ExtensionType extensionType,
        string name,
        string author,
        string description,
        string category,
        string gitHubRepository,
        string dSupportUserId,
        IList<ExtensionFile> extensionFiles)
    {
        PackageId = packageId;
        ExtensionType = extensionType;
        Name = name;
        Author = author;
        Description = description;
        Category = category;
        GitHubRepository = gitHubRepository;
        DSupportUserId = dSupportUserId;
        ExtensionFiles = extensionFiles;
    }
}