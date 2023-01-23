using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MacroDeckExtensionStoreLibrary.Enums;

namespace MacroDeckExtensionStoreLibrary.Models;

public class Extension
{
    public string PackageId { get; set; }
    public ExtensionType ExtensionType { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string GitHubRepository { get; set; }
    public string DSupportUserId { get; set; }
    public long Downloads { get; set; } = 0;
    public IList<ExtensionFile> ExtensionFiles { get; set; } = new List<ExtensionFile>();
}