namespace MacroDeckExtensionStoreLibrary.DataAccess.Entities;

public class ExtensionDownloadInfoEntity
{
    public int ExtensionDownloadId { get; set; }
    public ExtensionEntity ExtensionEntity { get; set; }
    public string DownloadedVersion { get; set; }
    public DateTime DownloadDateTime { get; set; }
}