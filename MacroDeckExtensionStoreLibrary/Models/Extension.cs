using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MacroDeckExtensionStoreLibrary.Enums;

namespace MacroDeckExtensionStoreLibrary.Models;

[Table("Extensions")]
public class Extension
{
    [Key, JsonIgnore] 
    public int ExtensionId { get; set; }
    
    public string PackageId { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExtensionType ExtensionType { get; set; }
    
    public string Name { get; set; }
    
    public string Author { get; set; }
    
    public string GitHubRepository { get; set; }
    
    public string DSupportUserId { get; set; }

    [JsonIgnore]
    public IList<ExtensionFile> ExtensionFiles { get; set; } = new List<ExtensionFile>();
}