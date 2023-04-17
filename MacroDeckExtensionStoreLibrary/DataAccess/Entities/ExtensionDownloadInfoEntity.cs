namespace MacroDeckExtensionStoreLibrary.DataAccess.Entities;

public class ExtensionDownloadInfoEntity
{
    public long ExtensionDownloadId { get; set; }
    public string DownloadedVersion { get; set; }
    public DateTime DownloadDateTime { get; set; }
    public long ExtensionId { get; set; }
    public virtual ExtensionEntity ExtensionEntity { get; set; }
}