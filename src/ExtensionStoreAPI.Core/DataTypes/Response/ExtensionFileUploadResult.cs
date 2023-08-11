using ExtensionStoreAPI.Core.DataTypes.MacroDeck;

namespace ExtensionStoreAPI.Core.DataTypes.Response;

public class ExtensionFileUploadResult
{
    public ExtensionManifest? ExtensionManifest { get; set; }
    public string? Readme { get; set; }
    public string? Description { get; set; }
    public string? PackageFileName { get; set; }
    public string? IconFileName { get; set; }
    public string? FileHash { get; set; }
    public string? LicenseName { get; set; }
    public string? LicenseUrl { get; set; }
    public bool Success { get; set; }
    public bool NewPlugin { get; set; }
    public string? CurrentVersion { get; set; }
    public string? NewVersion { get; set; }
}