namespace ExtensionStoreAPI.Config;

public class AppConfig : LoadableConfig<AppConfig>
{
    public string? ApiToken { get; set; }
}