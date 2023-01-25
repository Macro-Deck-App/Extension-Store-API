namespace MacroDeckExtensionStoreLibrary.Models;

public class ExtensionFileUploadResult
{
    public ExtensionManifest? ExtensionManifest { get; set; }
    public string? PackageFileName { get; set; }
    public string? IconFileName { get; set; }
    public string? ReadmeHtml { get; set; }
    public string? Description { get; set; }
    public string? MD5 { get; set; }
    public string? LicenseName { get; set; }
    public string? LicenseUrl { get; set; }
    public bool Success { get; set; }
}