using AutoMapper;
using MacroDeckExtensionStoreLibrary.DataAccess;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.Exceptions;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreLibrary.Repositories;

public class ExtensionsDbRepository
{
    private readonly ExtensionStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExtensionsDbRepository(ExtensionStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        dbContext.Database.EnsureCreated();
    }
    
    public async Task<Extension[]> GetExtensionsAsync()
    {
        var extensionEntities = await _dbContext.ExtensionEntities.ToArrayAsync();
        if (extensionEntities.Length == 0)
        {
            return Array.Empty<Extension>();
        }
        var extensions = _mapper.Map<Extension[]>(extensionEntities);
        return extensions;
    }

    public async Task<Extension?> GetExtensionByPackageIdAsync(string packageId)
    {
        var existingExtension = await _dbContext.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (existingExtension == null)
        {
            return null;
        }
        var extension = _mapper.Map<Extension>(existingExtension);
        return extension;
    }

    public async Task<ExtensionFile[]> GetExtensionFilesAsync(string packageId)
    {
        var extensionEntity = await _dbContext.ExtensionEntities
            .Include(x => x.ExtensionFiles)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extensionEntity == null)
        {
            return Array.Empty<ExtensionFile>();
        }

        var extensionFiles = _mapper.Map<ExtensionFile[]>(extensionEntity.ExtensionFiles);

        return extensionFiles;
    }

    public async Task<ExtensionFile?> GetExtensionFileAsync(string packageId, int apiVersion, string version = "latest")
    {
        var extensionFiles = await GetExtensionFilesAsync(packageId);
        var extensionFilesForApi = extensionFiles.Where(x => x.MinApiVersion <= apiVersion);

        if (!extensionFilesForApi.Any()) return null;

        var extensionFile = version == "latest"
            ? extensionFilesForApi.OrderBy(x => x.UploadDateTime).First()
            : extensionFilesForApi.FirstOrDefault(x => x.Version == version);

        return extensionFile;
    }

    public async Task CountDownloadAsync(string packageId)
    {
        var extension = await _dbContext.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (extension == null) return;
        extension.Downloads += 1;
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddExtensionFileAsync(string packageId, ExtensionFile extensionFile)
    {
        var existingExtension = await _dbContext.ExtensionEntities.Include(x => x.ExtensionFiles)
            .FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (existingExtension == null)
        {
            throw new PackageIdNotFoundException();
        }
        
        var extensionFiles = await GetExtensionFilesAsync(packageId);
        var existingExtensionFile = extensionFiles.FirstOrDefault(x =>
            x.MinApiVersion == extensionFile.MinApiVersion && x.Version == extensionFile.Version);
        if (existingExtensionFile != null)
        {
            throw new VersionAlreadyExistException();
        }

        var extensionFileEntity = _mapper.Map<ExtensionFileEntity>(extensionFile);
        
        existingExtension.ExtensionFiles.Add(extensionFileEntity);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddExtensionAsync(Extension extension)
    {
        var existingExtension = await _dbContext.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == extension.PackageId);
        if (existingExtension != null)
        {
            await UpdateExtensionAsync(extension);
            return;
        }

        var extensionEntity = _mapper.Map<ExtensionEntity>(extension);
        
        await _dbContext.ExtensionEntities.AddAsync(extensionEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteExtensionAsync(string packageId)
    {
        var existingExtension = await _dbContext.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == packageId);
        if (existingExtension == null)
        {
            return;
        }
        _dbContext.ExtensionEntities.Remove(existingExtension);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateExtensionAsync(Extension extension)
    {
        var existingExtension = await _dbContext.ExtensionEntities.FirstOrDefaultAsync(x => x.PackageId == extension.PackageId);
        if (existingExtension == null)
        {
            throw new KeyNotFoundException();
        }

        var extensionEntity = _mapper.Map<ExtensionEntity>(extension);
        
        _dbContext.Entry(existingExtension).CurrentValues.SetValues(extensionEntity);
        await _dbContext.SaveChangesAsync();
    }
}