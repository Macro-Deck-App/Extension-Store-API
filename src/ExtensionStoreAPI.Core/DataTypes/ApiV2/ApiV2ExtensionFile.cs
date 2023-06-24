namespace ExtensionStoreAPI.Core.DataTypes.ApiV2;

/// <summary>
/// DO NOT CHANGE ANYTHING BECAUSE OF COMPATIBILITY REASONS
/// </summary>

public class ApiV2ExtensionFile
{
    public string Version { get; set; }
    public int MinApiVersion { get; set; }
    public string PackageFileName { get; set; }
    public string IconFileName { get; set; }
    public string Readme { get; set; }
    public string FileHash { get; set; }
    public string LicenseName { get; set; }
    public string LicenseUrl { get; set; }
    public DateTime UploadDateTime { get; set; } = DateTime.Now;

    public ApiV2ExtensionFile(string version,
        int minApiVersion,
        string packageFileName,
        string iconFileName,
        string readme,
        string fileHash,
        string licenseName,
        string licenseUrl)
    {
        Version = version;
        MinApiVersion = minApiVersion;
        PackageFileName = packageFileName;
        IconFileName = iconFileName;
        Readme = readme;
        FileHash = fileHash;
        LicenseName = licenseName;
        LicenseUrl = licenseUrl;
    }
}