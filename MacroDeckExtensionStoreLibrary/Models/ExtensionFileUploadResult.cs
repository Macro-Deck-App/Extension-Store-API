namespace MacroDeckExtensionStoreLibrary.Models;

public class ExtensionFileUploadResult
{
    public ExtensionManifest? ExtensionManifest { get; set; }
    public string? ReadmeHtml { get; set; }
    public string? Description { get; set; }
    public string? Md5 { get; set; }
    public string? LicenseName { get; set; }
    public string? LicenseUrl { get; set; }
    public bool Success { get; set; }
    public bool NewPlugin { get; set; }
    public string? CurrentVersion { get; set; }
    public string NewVersion { get; set; }
}