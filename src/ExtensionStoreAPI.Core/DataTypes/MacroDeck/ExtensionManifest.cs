using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExtensionStoreAPI.Core.Enums;
using ExtensionStoreAPI.Core.Json;

namespace ExtensionStoreAPI.Core.DataTypes.MacroDeck;

public class ExtensionManifest
{
     [JsonPropertyName("type"), JsonConverter(typeof(JsonStringEnumConverter))]
     public ExtensionType? Type { get; set; } = ExtensionType.Plugin;

     [JsonPropertyName("name")]
     public string? Name { get; set; }

     [JsonPropertyName("author")]
     public string? Author { get; set; }

     [JsonPropertyName("category")]
     public string? Category { get; set; }

     [JsonPropertyName("author-discord-userid"), JsonConverter(typeof(StringToLongConverter))]
     public ulong? AuthorDiscordUserId { get; set; }

     [JsonPropertyName("repository")]
     public string? Repository { get; set; } = string.Empty;

     [JsonPropertyName("packageId")]
     public string? PackageId { get; set; }

     [JsonPropertyName("version")]
     public string? Version { get; set; }

     [JsonPropertyName("target-plugin-api-version")]
     public int? TargetPluginApiVersion { get; set; }

     [JsonPropertyName("dll")]
     public string? Dll { get; set; } = string.Empty;

     public static async Task<ExtensionManifest?> FromZipFilePathAsync(string zipFilePath)
     {
         var zipFile = ZipFile.OpenRead(zipFilePath);
         var extensionManifestFileEntry = zipFile.Entries.FirstOrDefault(x =>
             x.Name.Equals(Constants.ExtensionManifestFileName, StringComparison.InvariantCulture));
         
         if (extensionManifestFileEntry == null)
         {
             return null;
         }

         var fileEntryStream = extensionManifestFileEntry.Open();
         var fileEntryBaseStream = new StreamReader(fileEntryStream, Encoding.UTF8).BaseStream;
         var extensionManifest = await JsonSerializer.DeserializeAsync<ExtensionManifest>(fileEntryBaseStream);

         await fileEntryStream.DisposeAsync();
         await fileEntryBaseStream.DisposeAsync();
         zipFile.Dispose();
         return extensionManifest;
     }
}