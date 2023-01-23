using MacroDeckExtensionStoreLibrary.Enums;

namespace MacroDeckExtensionStoreLibrary.DataAccess.Entities;

public class ExtensionEntity
{
    public int ExtensionId { get; set; }
    
    public string PackageId { get; set; }
    
    public ExtensionType ExtensionType { get; set; }
    
    public string Name { get; set; }
    
    public string Author { get; set; }
    
    public string GitHubRepository { get; set; }
    
    public string DSupportUserId { get; set; }

    public long Downloads { get; set; } = 0;

    public ICollection<ExtensionFileEntity> ExtensionFiles { get; set; }
}