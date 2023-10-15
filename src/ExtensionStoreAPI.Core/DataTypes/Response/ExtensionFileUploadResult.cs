namespace ExtensionStoreAPI.Core.DataTypes.Response;

public class ExtensionFileUploadResult
{
    public string PackageId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Repository { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PackageFileName { get; set; } = string.Empty;
    public string IconFileName { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public string LicenseName { get; set; } = string.Empty;
    public string LicenseUrl { get; set; } = string.Empty;
    public bool Success { get; set; }
    public bool NewPlugin { get; set; }
    public string CurrentVersion { get; set; } = string.Empty;
    public string NewVersion { get; set; } = string.Empty;
    public int MinApiVersion { get; set; }
}