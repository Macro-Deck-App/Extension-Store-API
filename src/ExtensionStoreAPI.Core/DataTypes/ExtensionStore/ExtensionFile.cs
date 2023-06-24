namespace ExtensionStoreAPI.Core.DataTypes.ExtensionStore;

public class ExtensionFile
{
    public string Version { get; set; }
    public int MinApiVersion { get; set; }
    public string Readme { get; set; }
    public string FileHash { get; set; }
    public string LicenseName { get; set; }
    public string LicenseUrl { get; set; }
    public DateTime UploadDateTime { get; set; } = DateTime.Now;

    public ExtensionFile(string version,
        int minApiVersion,
        string readme,
        string fileHash,
        string licenseName,
        string licenseUrl)
    {
        Version = version;
        MinApiVersion = minApiVersion;
        Readme = readme;
        FileHash = fileHash;
        LicenseName = licenseName;
        LicenseUrl = licenseUrl;
    }
}