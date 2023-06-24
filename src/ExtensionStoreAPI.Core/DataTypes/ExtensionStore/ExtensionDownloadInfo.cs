namespace ExtensionStoreAPI.Core.DataTypes.ExtensionStore;

public class ExtensionDownloadInfo
{
    public string PackageId { get; set; }
    public string DownloadedVersion { get; set; }
    public DateTime DownloadDateTime { get; set; }

    public ExtensionDownloadInfo(string packageId, string downloadedVersion, DateTime downloadDateTime)
    {
        DownloadedVersion = downloadedVersion;
        DownloadDateTime = downloadDateTime;
        PackageId = packageId;
    }
}