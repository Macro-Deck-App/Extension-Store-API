using System.ComponentModel;
using ExtensionStoreAPI.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace ExtensionStoreAPI.Core.ErrorHandling;

public enum ErrorCodes
{
    [Description("Something went wrong, please contact the Macro Deck team")]
    [StatusCode(StatusCodes.Status500InternalServerError)]
    InternalError = -100500,
    
    [Description("The requested icon was not found")]
    [StatusCode(StatusCodes.Status404NotFound)]
    IconNotFound = -200100,
    
    [Description("The requested file was not found")]
    [StatusCode(StatusCodes.Status404NotFound)]
    FileNotFound = -300100,
        
    [Description("The requested version does not exist")]
    [StatusCode(StatusCodes.Status404NotFound)]
    VersionNotFound = -400100,
    
    [Description("The requested package id was not found")]
    [StatusCode(StatusCodes.Status404NotFound)]
    PackageIdNotFound = -500100,
    
    [Description("The uploaded file does not contain a ExtensionManifest.json")]
    [StatusCode(StatusCodes.Status400BadRequest)]
    ExtensionManifestNotFound = -600100,
    
    [Description("The ExtensionManifest.json of the uploaded file is invalid")]
    [StatusCode(StatusCodes.Status400BadRequest)]
    ExtensionManifestInvalid = -600110,
    
    [Description("This version already exists")]
    [StatusCode(StatusCodes.Status400BadRequest)]
    VersionAlreadyExists = -700100
}