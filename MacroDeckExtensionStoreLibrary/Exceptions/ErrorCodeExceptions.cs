using MacroDeckExtensionStoreLibrary.Enums;

namespace MacroDeckExtensionStoreLibrary.Exceptions;

public static class ErrorCodeExceptions
{
    public static ErrorCodeException InternalErrorException() => 
        new (500, "Something went wrong. Please contact the Macro Deck team", ErrorCode.InternalError);
    
    public static ErrorCodeException ExtensionManifestNotFoundException() => 
        new (404, "Extension Manifest was not found", ErrorCode.ExtensionManifestNotFound);
    
    public static ErrorCodeException VersionNotFoundException() => 
        new (404, "Version was not found", ErrorCode.VersionNotFound);
    
    public static ErrorCodeException PackageIdNotFoundException() => 
        new (404, "PackageId was not found", ErrorCode.PackageIdNotFound);
    
    public static ErrorCodeException VersionAlreadyExistsException() => 
        new (409, "Version already exists", ErrorCode.VersionAlreadyExists);
    
    public static ErrorCodeException IconNotFoundException() => 
        new (404, "Icon was not found", ErrorCode.IconNotFound);
    
    public static ErrorCodeException FileNotFoundException() => 
        new (404, "File was not found", ErrorCode.FileNotFound);
 
}