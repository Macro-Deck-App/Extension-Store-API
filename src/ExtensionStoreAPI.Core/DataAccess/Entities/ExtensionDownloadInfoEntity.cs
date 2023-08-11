namespace ExtensionStoreAPI.Core.DataAccess.Entities;

public class ExtensionDownloadInfoEntity : BaseCreatedEntity
{
    public string DownloadedVersion { get; set; } = string.Empty;
    public int ExtensionId { get; set; }
    public virtual ExtensionEntity? ExtensionEntity { get; set; }
}