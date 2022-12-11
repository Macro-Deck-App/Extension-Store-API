namespace MacroDeckExtensionStoreLibrary.Models;

public class ExtensionFileUploadResult
{
    public ExtensionManifest ExtensionManifest { get; init; }
    public string PackageFileName { get; init; }
    public string IconFileName { get; init; }
    
    public string MD5 { get; init; }
}