namespace MacroDeckExtensionStoreAPI.Config;

public class DatabaseConfig : LoadableConfig<DatabaseConfig>
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public string ToConnectionString()
    {
        return  $"Host={Host}; " +
                $"Database={Database}; " +
                $"Username={User}; " +
                $"Password={Password};";
    }
}