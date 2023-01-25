namespace MacroDeckExtensionStoreLibrary;

public static class Paths
{
    public static string MainDirectory
    {
        get
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            return string.IsNullOrWhiteSpace(environment) 
                ? "__testConfigs__" 
                : "/etc/extensionstoreapi";
        }
    }

    public static readonly string DatabaseConfigPath = Path.Combine(MainDirectory, "Database.json");
    public static readonly string AppConfigPath = Path.Combine(MainDirectory, "Config.json");
    public static readonly string DataDirectory = Path.Combine(MainDirectory, "Data");
    public static readonly string TempDirectory = Path.Combine(MainDirectory, "Temp");

    public static void EnsureDirectoriesCreated()
    {
        CheckAndCreateDirectory(MainDirectory);
        CheckAndCreateDirectory(DataDirectory);
        CheckAndCreateDirectory(TempDirectory);
        static void CheckAndCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }
    }
}