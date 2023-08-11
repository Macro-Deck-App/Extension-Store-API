namespace ExtensionStoreAPI.Core.ManagerInterfaces;

public interface IFileManager
{
    public void DeleteExtensionFile(string packageFileName, string iconFileName);
}