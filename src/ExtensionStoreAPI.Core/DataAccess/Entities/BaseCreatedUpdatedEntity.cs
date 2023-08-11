namespace ExtensionStoreAPI.Core.DataAccess.Entities;

public class BaseCreatedUpdatedEntity : BaseCreatedEntity
{
    public DateTime? UpdatedTimestamp { get; set; }
}