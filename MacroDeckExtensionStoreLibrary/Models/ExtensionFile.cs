using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MacroDeckExtensionStoreLibrary.Models;

[Table("ExtensionFiles")]
public class ExtensionFile
{
    [Key, ForeignKey(nameof(Extension)), JsonIgnore] 
    public int ExtensionFileId { get; set; }
    
    public string Version { get; set; }
    
    public int MinAPIVersion { get; set; }
    
    public string FileName { get; set; }
    
    public string IconFileName { get; set; }
    
    public string DescriptionMarkup { get; set; }
    
    public string MD5Hash { get; set; }
    
    public string License { get; set; }
    
}