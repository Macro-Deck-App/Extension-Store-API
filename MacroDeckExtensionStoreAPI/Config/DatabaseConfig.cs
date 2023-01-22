namespace MacroDeckExtensionStoreAPI.Config;

public class DatabaseConfig : LoadableConfig<DatabaseConfig>
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public string ToConnectionString()
    {
        return  $"server={Host}; " +
                $"port={Port}; " +
                $"database={Database}; " +
                $"user={User}; " +
                $"password={Password}; " +
                "Persist Security Info=False; Connect Timeout=300";
    }
}