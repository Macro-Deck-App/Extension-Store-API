namespace ExtensionStoreAPI.Core.DataAccess.Entities;

public class ExtensionFileEntity : BaseCreatedEntity
{
    public string Version { get; set; } = string.Empty;
    public int MinApiVersion { get; set; }
    public string PackageFileName { get; set; } = string.Empty;
    public string IconFileName { get; set; } = string.Empty;
    public string? Readme { get; set; }
    public string FileHash { get; set; } = string.Empty;
    public string? LicenseName { get; set; }
    public string? LicenseUrl { get; set; }
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    public int ExtensionId { get; set; }
    public ExtensionEntity? ExtensionEntity { get; set; }
}