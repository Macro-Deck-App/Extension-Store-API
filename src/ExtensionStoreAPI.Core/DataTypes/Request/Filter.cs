namespace ExtensionStoreAPI.Core.DataTypes.Request;

public class Filter
{
    public bool ShowPlugins { get; set; } = true;
    public bool ShowIconPacks { get; set; } = true;
    public string? Category { get; set; }
}