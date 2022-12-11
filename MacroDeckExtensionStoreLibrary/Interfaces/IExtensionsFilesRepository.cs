using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.Interfaces;

public interface IExtensionsFilesRepository
{
    
    public string BasePath { get; }
    public string TmpPath { get; }
    public string ExtensionsPath { get; }
    public string GenerateUniqueFileName(ExtensionManifest extensionManifest);
    public Task<ExtensionFileUploadResult> SaveExtensionFileFromStreamAsync(Stream uploadStream);
    public Task DeleteExtensionFileAsync(ExtensionFile extensionFile);
    public Task<byte[]?> GetExtensionFileBytesAsync(ExtensionFile extensionFile);
}