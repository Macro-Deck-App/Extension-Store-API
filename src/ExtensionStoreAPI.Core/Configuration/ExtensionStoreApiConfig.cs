namespace ExtensionStoreAPI.Core.Configuration;

public partial class ExtensionStoreApiConfig
{
    public static string DatabaseConnectionString => GetString("database:connection_string");
    public static string? DatabaseConnectionStringOverride { get; set; }
    public static string AdminAuthenticationToken => GetString("authentication:admin_token");
    public static string MacroBotLoggingUrl => GetString("logging:macro-bot:url");
    public static string MacroBotLoggingToken => GetString("logging:macro-bot:token");
}