using MacroDeckExtensionStoreLibrary.Enums;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using MacroDeckExtensionStoreLibrary.Utils;

namespace MacroDeckExtensionStoreLibrary.Repositories;

public class ExtensionsFilesFileRepository : IExtensionsFilesRepository
{
    public string BasePath { get; }
    public string TmpPath { get; }
    public string ExtensionsPath { get; }
    
    public ExtensionsFilesFileRepository(string path)
    {
        BasePath = Path.GetFullPath(path);
        TmpPath = Path.Combine(BasePath, "Temp");
        ExtensionsPath = Path.Combine(BasePath, "Extensions");
        CheckPaths();
    }

    private void CheckPaths()
    {
        CheckAndCreateDirectory(BasePath);
        CheckAndCreateDirectory(TmpPath);
        CheckAndCreateDirectory(ExtensionsPath);
    }

    private static void CheckAndCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public async Task<byte[]?> GetExtensionFileBytesAsync(ExtensionFile extensionFile)
    {
        var filePath = Path.Combine(ExtensionsPath, extensionFile.PackageFileName);
        if (!File.Exists(filePath)) return null;
        var bytes = await File.ReadAllBytesAsync(filePath);
        return bytes;
    }

    public async Task DeleteExtensionFileAsync(ExtensionFile extensionFile)
    {
        var packageFilePath = Path.Combine(ExtensionsPath, extensionFile.PackageFileName);
        var iconFilePath = Path.Combine(ExtensionsPath, extensionFile.IconFileName);
        try
        {
            DeleteFile(packageFilePath);
        }
        catch { /* Ignored */ }
        try
        {
            DeleteFile(iconFilePath);
        }
        catch { /* Ignored */ }
    }

    private static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public async Task<ExtensionFileUploadResult> SaveExtensionFileFromStreamAsync(Stream uploadStream)
    {
        var tmpFilePath = Path.Combine(TmpPath, Guid.NewGuid().ToString());
        await using var tmpFileStream = File.Create(tmpFilePath);
        uploadStream.Seek(0, SeekOrigin.Begin);
        await uploadStream.CopyToAsync(tmpFileStream);
        tmpFileStream.Close();
        if (!File.Exists(tmpFilePath)) throw new FileNotFoundException();
        
        var manifest = await ExtensionManifest.FromZipFilePathAsync(tmpFilePath);
        if (manifest == null) throw new InvalidDataException();
        var finalFileName = GenerateUniqueFileName(manifest);
        var packageFileName = $"{finalFileName}.zip";
        var iconFileName = $"{finalFileName}.png";

        Stream? iconMemoryStream;
        if (manifest.Type == ExtensionType.Plugin)
        {
            iconMemoryStream = await ExtensionIconExtractor.FromZipFilePathAsync(tmpFilePath);
        }
        else
        {
            iconMemoryStream = await ExtensionIconExtractor.GeneratePreviewImage(tmpFilePath);
        }

        if (iconMemoryStream == null) throw new InvalidDataException();
        await using var iconFileStream = File.Create(Path.Combine(ExtensionsPath, iconFileName));
        iconMemoryStream.Seek(0, SeekOrigin.Begin);
        await iconMemoryStream.CopyToAsync(iconFileStream);
        iconMemoryStream.Close();
        iconFileStream.Close();
        
        var finalPackageFilePath = Path.Combine(ExtensionsPath, packageFileName);

        File.Copy(tmpFilePath, finalPackageFilePath);

        var md5 = await MD5Util.GetMD5HashAsync(finalPackageFilePath);
        
        try
        {
            File.Delete(tmpFilePath);
        }
        catch (Exception)
        {
            // ignored
        }

        var result = new ExtensionFileUploadResult()
        {
            ExtensionManifest = manifest,
            PackageFileName = packageFileName,
            IconFileName = iconFileName,
            MD5 = md5
        };
        return result;
    }


    public string GenerateUniqueFileName(ExtensionManifest extensionManifest)
    {
        var guid = Guid.NewGuid().ToString();
        var fileName = $"{extensionManifest.PackageId}_{extensionManifest.Version}_{guid}";
        return fileName;
    }
}