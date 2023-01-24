namespace MacroDeckExtensionStoreLibrary.Models;

public class ExtensionFileUploadResult
{
    public ExtensionManifest ExtensionManifest { get; set; }
    public string PackageFileName { get; set; }
    public string IconFileName { get; set; }
    public string MD5 { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}