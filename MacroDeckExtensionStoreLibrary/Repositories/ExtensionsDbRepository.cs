using MacroDeckExtensionStoreLibrary.DataAccess;
using MacroDeckExtensionStoreLibrary.Exceptions;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.Repositories;

public class ExtensionsDbRepository : IExtensionsRepository
{
    private readonly ExtensionStoreDbContext _dbContext;

    public ExtensionsDbRepository(ExtensionStoreDbContext dbContext)
    {
        _dbContext = dbContext;
        dbContext.Database.EnsureCreated();
    }
    
    public async Task<Extension[]> GetExtensionsAsync()
    {
        var extensions = await _dbContext.Extensions.ToArrayAsync();
        return extensions;
    }

    public async Task<Extension?> GetExtensionByPackageIdAsync(string packageId)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == packageId);
        return existingExtension;
    }

    public async Task<ExtensionFile[]> GetExtensionFilesAsync(string packageId)
    {
        var extension = await _dbContext.Extensions
            .Include(x => x.ExtensionFiles)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        return extension?.ExtensionFiles.ToArray() ?? Array.Empty<ExtensionFile>();
    }

    public async Task<ExtensionFile?> GetExtensionFileAsync(string packageId, int apiVersion, string version = "latest")
    {
        var extensionFiles = await GetExtensionFilesAsync(packageId);
        var extensionFilesForApi = extensionFiles.Where(x => x.MinAPIVersion <= apiVersion);

        if (!extensionFilesForApi.Any()) return null;

        var extensionFile = version == "latest"
            ? extensionFilesForApi.OrderBy(x => x.UploadDateTime).First()
            : extensionFilesForApi.FirstOrDefault(x => x.Version == version);

        return extensionFile;
    }

    public async Task CountDownloadAsync(string packageId)
    {
        var extension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extension == null) return;
        extension.Downloads += 1;
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddExtensionFileAsync(string packageId, ExtensionFile extensionFile)
    {
        var existingExtension = await _dbContext.Extensions.Include(x => x.ExtensionFiles)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (existingExtension == null)
        {
            throw new PackageIdNotFoundException();
        }
        
        var extensionFiles = await GetExtensionFilesAsync(packageId);
        var existingExtensionFile = extensionFiles.FirstOrDefault(x =>
            x.MinAPIVersion == extensionFile.MinAPIVersion && x.Version == extensionFile.Version);
        if (existingExtensionFile != null)
        {
            throw new VersionAlreadyExistException();
        }
        
        existingExtension.ExtensionFiles.Add(extensionFile);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddExtensionAsync(Extension extension)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == extension.PackageId);
        if (existingExtension != null)
        {
            await UpdateExtensionAsync(extension);
            return;
        }
        await _dbContext.Extensions.AddAsync(extension);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteExtensionAsync(string packageId)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (existingExtension == null)
        {
            return;
        }
        _dbContext.Extensions.Remove(existingExtension);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateExtensionAsync(Extension extension)
    {
        var existingExtension = await _dbContext.Extensions.FirstOrDefaultAsync(x => x.PackageId == extension.PackageId);
        if (existingExtension == null)
        {
            throw new KeyNotFoundException();
        }
        extension.ExtensionId = existingExtension.ExtensionId;
        _dbContext.Entry(existingExtension).CurrentValues.SetValues(extension);
        await _dbContext.SaveChangesAsync();
    }
}