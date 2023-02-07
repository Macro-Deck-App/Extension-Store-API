using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.ManagerInterfaces;

public interface IFileManager
{
    public void DeleteExtensionFile(string packageFileName, string iconFileName);
}