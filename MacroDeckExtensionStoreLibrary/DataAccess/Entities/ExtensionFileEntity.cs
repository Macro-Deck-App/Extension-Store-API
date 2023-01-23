namespace MacroDeckExtensionStoreLibrary.DataAccess.Entities;

public class ExtensionFileEntity
{
    public int ExtensionFileId { get; set; }
    
    public string Version { get; set; }
    
    public int MinApiVersion { get; set; }
    
    public string PackageFileName { get; set; }
    
    public string IconFileName { get; set; }
    
    public string DescriptionHtml { get; set; }
    
    public string MD5Hash { get; set; }
    
    public string LicenseName { get; set; }
    
    public string LicenseUrl { get; set; }
    
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    
    public ExtensionEntity ExtensionEntity { get; set; }
}