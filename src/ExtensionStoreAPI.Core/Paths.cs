namespace ExtensionStoreAPI.Core;

public static class Paths
{
    private static string MainDirectory => "storage";
    public static readonly string DataDirectory = Path.Combine(MainDirectory, "data");
    public static readonly string TempDirectory = Path.Combine(MainDirectory, "temp");

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