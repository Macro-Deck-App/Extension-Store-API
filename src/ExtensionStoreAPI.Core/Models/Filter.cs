namespace ExtensionStoreAPI.Core.Models;

public class Filter
{
    public bool ShowPlugins { get; set; } = true;
    public bool ShowIconPacks { get; set; } = true;
    public string? Category { get; set; }
}