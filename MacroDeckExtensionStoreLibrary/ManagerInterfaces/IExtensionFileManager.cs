using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.ManagerInterfaces;

public interface IExtensionFileManager
{
    public Task<bool> ExistAsync(string packageId, string version);
    public Task<ExtensionFile[]> GetFilesAsync(string packageId);
    public Task<ExtensionFile> GetFileAsync(string packageId, int? targetApiVersion = null, string version = "latest");
    public Task<ExtensionFileUploadResult> CreateFileAsync(Stream packageStream);
    public Task DeleteFileAsync(string packageId, string version);
    public Task<byte[]> GetFileBytesAsync(string packageId, int targetApiVersion, string version);
}