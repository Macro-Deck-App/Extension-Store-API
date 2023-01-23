using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MacroDeckExtensionStoreLibrary.Models;

public class ExtensionFile
{
    public string Version { get; set; }
    public int MinApiVersion { get; set; }
    public string PackageFileName { get; set; }
    public string IconFileName { get; set; }
    public string DescriptionHtml { get; set; }
    public string MD5Hash { get; set; }
    public string LicenseName { get; set; }
    public string LicenseUrl { get; set; }
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    
}