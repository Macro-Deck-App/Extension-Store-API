#pragma warning disable CS8618
namespace MacroDeckExtensionStoreLibrary.Models.ApiV2;

/// <summary>
/// DO NOT CHANGE ANYTHING BECAUSE OF COMPATIBILITY REASONS
/// </summary>

public class ApiV2ExtensionFile
{
    public string Version { get; set; }
    public int MinApiVersion { get; set; }
    public string PackageFileName { get; set; }
    public string IconFileName { get; set; }
    public string DescriptionHtml { get; set; }
    public string Md5Hash { get; set; }
    public string LicenseName { get; set; }
    public string LicenseUrl { get; set; }
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    
}