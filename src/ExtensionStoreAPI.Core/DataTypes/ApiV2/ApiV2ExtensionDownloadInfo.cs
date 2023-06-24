namespace ExtensionStoreAPI.Core.DataTypes.ApiV2;

/// <summary>
/// DO NOT CHANGE ANYTHING BECAUSE OF COMPATIBILITY REASONS
/// </summary>

public class ApiV2ExtensionDownloadInfo
{
    public string DownloadedVersion { get; set; }
    public DateTime DownloadDateTime { get; set; }

    public ApiV2ExtensionDownloadInfo(string downloadedVersion, DateTime downloadDateTime)
    {
        DownloadedVersion = downloadedVersion;
        DownloadDateTime = downloadDateTime;
    }
}