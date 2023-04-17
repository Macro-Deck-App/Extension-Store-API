using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using Serilog;

namespace MacroDeckExtensionStoreLibrary.Managers;

public class FileManager : IFileManager
{
    private static readonly ILogger Logger = Log.ForContext(typeof(FileManager));
    
    public void DeleteExtensionFile(string packageFileName, string iconFileName)
    {
        var packageFilePath = Path.Combine(Paths.DataDirectory, packageFileName);
        var iconFilePath = Path.Combine(Paths.DataDirectory, iconFileName);
        var packageFileDeleted = TryDeleteAsync(packageFilePath);
        var iconFileDeleted = TryDeleteAsync(iconFilePath);
        Logger.Verbose("Package file deleted: {Deleted}", packageFileDeleted);
        Logger.Verbose("Icon file deleted: {Deleted}", iconFileDeleted);
    }

    private static bool TryDeleteAsync(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            File.Delete(path);
            return !Path.Exists(path);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to delete file");
            return false;
        }
    }
}