#pragma warning disable CS8618
namespace ExtensionStoreAPI.Core.Models;

public class ExtensionFile
{
    public string Version { get; set; }
    public int MinApiVersion { get; set; }
    public string DescriptionHtml { get; set; }
    public string Md5Hash { get; set; }
    public string LicenseName { get; set; }
    public string LicenseUrl { get; set; }
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    
}