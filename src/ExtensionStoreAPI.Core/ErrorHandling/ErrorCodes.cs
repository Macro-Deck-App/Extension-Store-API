using System.ComponentModel;
using ExtensionStoreAPI.Core.Attributes;

namespace ExtensionStoreAPI.Core.ErrorHandling;

public enum ErrorCodes
{
    [Description("Something went wrong, please contact the Macro Deck team")]
    [StatusCode(500)]
    InternalError = -100500,
    
    [Description("The requested icon was not found")]
    [StatusCode(404)]
    IconNotFound = -200100,
    
    [Description("The requested file was not found")]
    [StatusCode(404)]
    FileNotFound = -300100,
        
    [Description("The requested version does not exist")]
    [StatusCode(404)]
    VersionNotFound = -400100,
    
    [Description("The requested package id was not found")]
    [StatusCode(404)]
    PackageIdNotFound = -500100,
    
    [Description("The uploaded file does not contain a ExtensionManifest.json")]
    [StatusCode(400)]
    ExtensionManifestNotFound = -600100,
    
    [Description("The ExtensionManifest.json of the uploaded file is invalid")]
    [StatusCode(400)]
    ExtensionManifestInvalid = -600110,
    
    [Description("This version already exists")]
    [StatusCode(400)]
    VersionAlreadyExists = -700100
}