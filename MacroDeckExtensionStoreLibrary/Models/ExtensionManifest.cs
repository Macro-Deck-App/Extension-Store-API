using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MacroDeckExtensionStoreLibrary.Enums;

namespace MacroDeckExtensionStoreLibrary.Models;

public class ExtensionManifest
{
     [JsonPropertyName("type"), JsonConverter(typeof(JsonStringEnumConverter))]
     public ExtensionType Type { get; set; } = ExtensionType.Plugin;

     [JsonPropertyName("name")]
     public string Name { get; set; }

     [JsonPropertyName("author")]
     public string Author { get; set; }

     [JsonPropertyName("author-discord-userid")]
     public string AuthorDiscordUserId { get; set; }

     [JsonPropertyName("repository")]
     public string Repository { get; set; } = "";

     [JsonPropertyName("packageId")]
     public string PackageId { get; set; }

     [JsonPropertyName("version")]
     public string Version { get; set; }

     [JsonPropertyName("target-plugin-api-version")]
     public int TargetPluginAPIVersion { get; set; } = 31;

     [JsonPropertyName("dll")]
     public string Dll { get; set; } = "";

     public static string SerializeAsync(ExtensionManifest extensionManifest)
     {
         var serialized = JsonSerializer.Serialize(extensionManifest);
         return serialized;
     }

     public static async Task<ExtensionManifest?> FromZipFilePathAsync(string zipFilePath)
     {
         var zipFile = ZipFile.OpenRead(zipFilePath);
         var extensionManifestFileEntry = zipFile.Entries.FirstOrDefault(x => x.Name.Equals(Constants.ExtensionManifestFileName, StringComparison.InvariantCulture));
         if (extensionManifestFileEntry == null) return null;
         await using var stream = new StreamReader(extensionManifestFileEntry.Open(), Encoding.UTF8).BaseStream;
         var extensionManifest = await FromJsonStreamAsync(stream);
         zipFile.Dispose();
         return extensionManifest;
     }

     public static async Task<ExtensionManifest?> FromJsonStreamAsync(Stream stream)
     {
         var extensionManifest = await JsonSerializer.DeserializeAsync<ExtensionManifest>(stream);
         stream.Close();
         return extensionManifest;
     }
}